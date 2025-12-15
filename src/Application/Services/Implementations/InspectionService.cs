using Application.DTOs.DevicesDTOs;
using Application.DTOs.InspectionDTOs;
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
        private readonly IUserRepository _userRepo;
        private readonly IDecommissioningRequestRepository _decommissioningRepo;
        private readonly IUnitOfWork _unitOfWork;

        public InspectionService(
            IReceivingInspectionRequestRepository inspectionRepo,
            IDeviceRepository deviceRepo,
            IDepartmentRepository departmentRepo,
            ISectionRepository sectionRepo,
            IDecommissioningRequestRepository decommissioningRepo,
            IUserRepository userRepo,
            IUnitOfWork unitOfWork
        )
        {
            _inspectionRepo = inspectionRepo;
            _deviceRepo = deviceRepo;
            _departmentRepo = departmentRepo;
            _sectionRepo = sectionRepo;
            _decommissioningRepo = decommissioningRepo;
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
        }

        #region Queries - Technician

        public async Task<IEnumerable<ReceivingInspectionRequestDto>> GetInspectionRequestsByTechnicianAsync(int technicianId)
        {
            var inspections = await _inspectionRepo.GetReceivingInspectionRequestsByTechnicianAsync(technicianId);
            return await MapToDtoList(inspections);
        }

        public async Task<IEnumerable<ReceivingInspectionRequestDto>> GetPendingInspectionsByTechnicianAsync(int technicianId)
        {
            var inspections = await _inspectionRepo.GetReceivingInspectionRequestsByTechnicianAsync(technicianId);
            return await MapToDtoList(inspections
                .Where(i => i.Status == InspectionRequestStatus.Pending));

        }

        #endregion

        #region Queries - Administrator

        public async Task<IEnumerable<ReceivingInspectionRequestDto>> GetInspectionRequestsByAdminAsync(int adminId)
        {
            var inspections = await _inspectionRepo.GetInspectionRequestsByAdminAsync(adminId);
            return await MapToDtoList(inspections);
        }

        public async Task<IEnumerable<DeviceDto>> GetRevisedDevicesByAdminAsync(int adminId)
        {
            var inspections = await _inspectionRepo.GetInspectionRequestsByAdminAsync(adminId);
            var pendingInspections = inspections.Where(i => i.IsAccepted() || i.IsRejected()).ToList();

            if (!pendingInspections.Any()) return Enumerable.Empty<DeviceDto>();

            // Cargar todos los dispositivos necesarios
            var deviceIds = pendingInspections.Select(i => i.DeviceId).Distinct();
            var devices = new Dictionary<int, Domain.Entities.Device>();

            foreach (var deviceId in deviceIds)
            {
                var device = await _deviceRepo.GetByIdAsync(deviceId);
                if (device != null && device.OperationalState == OperationalState.Revised) devices[deviceId] = device;
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
            System.Console.WriteLine("Inspection ready");
            ValidateTechnicianAuthorization(request.TechnicianId, inspectionRequest.TechnicianId);
            System.Console.WriteLine("Technician Validation");
            var device = await _deviceRepo.GetByIdAsync(request.DeviceId)
                ?? throw new EntityNotFoundException("Device", request.DeviceId);
            System.Console.WriteLine($"Get device {device.Name}");
            device.UpdateOperationalState(OperationalState.Revised);
            await _deviceRepo.UpdateAsync(device);
            System.Console.WriteLine("DeviceOperationalState updated");

            if (request.IsApproved)
            {
                inspectionRequest.Accept();
            }
            else
            {
                await HandleRejectionAsync(inspectionRequest, device, request);
            }
            System.Console.WriteLine("Approved");

            await _inspectionRepo.UpdateAsync(inspectionRequest);
            await _unitOfWork.SaveChangesAsync();
            System.Console.WriteLine("Done");
        }

        #endregion

        #region Private Helper Methods

        private async Task<ReceivingInspectionRequestDto> MapToDto(ReceivingInspectionRequest inspection)
        {

            var deviceName = (await _deviceRepo.GetByIdAsync(inspection.DeviceId))?.Name ?? "Unknown Device";
            System.Console.WriteLine(deviceName);
            System.Console.WriteLine(inspection.AdministratorId);
            var userName = (await _userRepo.GetByIdAsync(inspection.AdministratorId))?.FullName ?? "Unknown Admin";
            System.Console.WriteLine(userName);
            var technicianFullName = (await _userRepo.GetByIdAsync(inspection.TechnicianId))?.FullName ?? "Unknown Technician";
            System.Console.WriteLine(technicianFullName);
            return new ReceivingInspectionRequestDto(
                inspection.ReceivingInspectionRequestId,
                inspection.EmissionDate,
                inspection.DeviceId,
                deviceName,
                inspection.AdministratorId,
                userName,
                inspection.TechnicianId,
                technicianFullName,
                inspection.Status,
                inspection.RejectReason
            );
        }
        private async Task<List<ReceivingInspectionRequestDto>> MapToDtoList(IEnumerable<ReceivingInspectionRequest> inspections)
        {
            var dtoList = new List<ReceivingInspectionRequestDto>();
            foreach (var inspection in inspections)
            {
                var dto = await MapToDto(inspection);
                dtoList.Add(dto);
            }
            return dtoList;
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
            inspectionRequest.Reject((DecommissioningReason)request.Reason);

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
                (DecommissioningReason)request.Reason
            );

            await _decommissioningRepo.AddAsync(decommissioningRequest);
        }

        #endregion
    }
}
