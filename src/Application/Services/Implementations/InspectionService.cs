using Application.DTOs.Inventory;
using Application.Services.Interfaces;
using Domain.Aggregations;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services.Implementations
{
    /// <summary>
    /// Servicio responsable de las operaciones de inspección de dispositivos.
    /// </summary>
    public class InspectionService : IInspectionService
    {
        private readonly IReceivingInspectionRequestRepository _inspectionRepo;
        private readonly IDeviceRepository _deviceRepo;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly ISectionRepository _sectionRepo;
        private readonly IDecommissioningRequestRepository _decommissioningRepo;
        private readonly IUnitOfWork _unitOfWork;

        public InspectionService(
            IReceivingInspectionRequestRepository inspectionRepo,
            IDeviceRepository deviceRepo,
            IDepartmentRepository departmentRepo,
            ISectionRepository sectionRepo,
            IDecommissioningRequestRepository decommissioningRepo,
            IUnitOfWork unitOfWork
        )
        {
            _inspectionRepo = inspectionRepo;
            _deviceRepo = deviceRepo;
            _departmentRepo = departmentRepo;
            _sectionRepo = sectionRepo;
            _decommissioningRepo = decommissioningRepo;
            _unitOfWork = unitOfWork;
        }

        #region Queries - Technician

        public async Task<IEnumerable<ReceivingInspectionRequestDto>> GetInspectionRequestsByTechnicianAsync(int technicianId)
        {
            var inspections = await _inspectionRepo.GetReceivingInspectionRequestsByTechnicianAsync(technicianId);
            return inspections.Select(MapToDto);
        }

        public async Task<IEnumerable<ReceivingInspectionRequestDto>> GetPendingInspectionsByTechnicianAsync(int technicianId)
        {
            var inspections = await _inspectionRepo.GetReceivingInspectionRequestsByTechnicianAsync(technicianId);
            return inspections
                .Where(i => i.Status == InspectionRequestStatus.Pending)
                .Select(MapToDto);
        }

        #endregion

        #region Queries - Administrator

        public async Task<IEnumerable<ReceivingInspectionRequestDto>> GetInspectionRequestsByAdminAsync(int adminId)
        {
            var inspections = await _inspectionRepo.GetInspectionRequestsByAdminAsync(adminId);
            return inspections.Select(MapToDto);
        }

        public async Task<IEnumerable<DeviceDto>> GetRevisedDevicesByAdminAsync(int adminId)
        {
            var inspections = await _inspectionRepo.GetInspectionRequestsByAdminAsync(adminId);
            var pendingInspections = inspections.Where(i => i.IsPending()).ToList();

            if (!pendingInspections.Any()) return Enumerable.Empty<DeviceDto>();

            // Cargar todos los dispositivos necesarios
            var deviceIds = pendingInspections.Select(i => i.DeviceId).Distinct();
            var devices = new Dictionary<int, Domain.Entities.Device>();
            
            foreach (var deviceId in deviceIds)
            {
                var device = await _deviceRepo.GetByIdAsync(deviceId);
                if (device != null) devices[deviceId] = device;
            }

            // Cargar todos los departamentos necesarios 
            var departmentIds = devices.Values.Select(d => d.DepartmentId).Distinct();
            var departments = new Dictionary<int, Domain.Entities.Department>();
            
            foreach (var deptId in departmentIds)
            {
                var dept = await _departmentRepo.GetByIdAsync(deptId);
                if (dept != null) departments[deptId] = dept;
            }

            // Crear DTOs usando los diccionarios cargados
            return pendingInspections
                .Where(i => devices.ContainsKey(i.DeviceId))
                .Select(i =>
                {
                    var device = devices[i.DeviceId];
                    var department = departments.GetValueOrDefault(device.DepartmentId);
                    
                    return new DeviceDto(
                        device.DeviceId,
                        device.Name,
                        device.Type,
                        device.OperationalState,
                        department?.Name ?? "Unassigned"
                    );
                })
                .ToList();
        }

        #endregion

        #region Commands

        public async Task AssignDeviceForInspectionAsync(AssignDeviceForInspectionRequestDto request)
        {
            var device = await _deviceRepo.GetByIdAsync(request.DeviceId)
                ?? throw new EntityNotFoundException("Device", request.DeviceId);

            device.UpdateOperationalState(OperationalState.UnderRevision);
            await _deviceRepo.UpdateAsync(device);

            var inspectionRequest = new ReceivingInspectionRequest(
                DateTime.Now,
                request.DeviceId,
                request.AdministratorId,
                request.TechnicianId
            );

            await _inspectionRepo.AddAsync(inspectionRequest);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ProcessInspectionDecisionAsync(InspectionDecisionRequestDto request)
        {
            var inspectionRequest = await _inspectionRepo.GetReceivingInspectionRequestsByDeviceAsync(request.DeviceId)
                ?? throw new EntityNotFoundException("InspectionRequest", request.DeviceId);

            ValidateTechnicianAuthorization(request.TechnicianId, inspectionRequest.TechnicianId);

            var device = await _deviceRepo.GetByIdAsync(request.DeviceId)
                ?? throw new EntityNotFoundException("Device", request.DeviceId);

            device.UpdateOperationalState(OperationalState.Revised);
            await _deviceRepo.UpdateAsync(device);

            if (request.IsApproved)
            {
                inspectionRequest.Accept();
            }
            else
            {
                await HandleRejectionAsync(inspectionRequest, device, request);
            }

            await _inspectionRepo.UpdateAsync(inspectionRequest);
            await _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region Private Helper Methods

        private static ReceivingInspectionRequestDto MapToDto(ReceivingInspectionRequest inspection)
        {
            return new ReceivingInspectionRequestDto(
                inspection.ReceivingInspectionRequestId,
                inspection.EmissionDate,
                inspection.DeviceId,
                inspection.AdministratorId,
                inspection.TechnicianId,
                inspection.Status,
                inspection.RejectReason
            );
        }

        private static void ValidateTechnicianAuthorization(int requestTechnicianId, int assignedTechnicianId)
        {
            if (requestTechnicianId != assignedTechnicianId)
            {
                throw new AccessDeniedException(
                    $"Technician with id {requestTechnicianId} is not authorized to process this inspection request"
                );
            }
        }

        private async Task HandleRejectionAsync(
            ReceivingInspectionRequest inspectionRequest,
            Domain.Entities.Device device,
            InspectionDecisionRequestDto request)
        {
            inspectionRequest.Reject(request.Reason);

            var department = await _departmentRepo.GetByIdAsync(device.DepartmentId)
                ?? throw new EntityNotFoundException("Department", device.DepartmentId);

            var section = await _sectionRepo.GetByIdAsync(department.SectionId)
                ?? throw new EntityNotFoundException("Section", department.SectionId);

            if (!section.SectionManagerId.HasValue)
            {
                throw new BusinessRuleViolationException(
                    $"Section {section.SectionId} does not have an assigned manager to receive the decommissioning request"
                );
            }

            var decommissioningRequest = new DecommissioningRequest(
                request.TechnicianId,
                request.DeviceId,
                DateTime.Now,
                request.Reason
            );

            await _decommissioningRepo.AddAsync(decommissioningRequest);
        }

        #endregion
    }
}
