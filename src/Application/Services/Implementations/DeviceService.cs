using Application.DTOs.Decommissioning;
using Application.DTOs.DevicesDTOs;
using Application.DTOs.Maintenance;
using Application.DTOs.Transfer;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services.Implementations
{
    /// <summary>
    /// Servicio responsable de las operaciones CRUD de dispositivos.
    /// </summary>
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepo;
        private readonly IUserRepository _userRepo;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly ISectionRepository _sectionRepo;
        private readonly IMaintenanceRecordRepository _maintenanceRepo;
        private readonly ITransferRepository _transferRepo;
        private readonly IDecommissioningRequestRepository _decommissioningRepo;
        private readonly IReceivingInspectionRequestRepository _inspectionRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceService(
            IDeviceRepository deviceRepo,
            IUserRepository userRepo,
            IDepartmentRepository departmentRepo,
            ISectionRepository sectionRepo,
            IMaintenanceRecordRepository maintenanceRepo,
            ITransferRepository transferRepo,
            IDecommissioningRequestRepository decommissioningRepo,
            IReceivingInspectionRequestRepository inspectionRepo,
            IUnitOfWork unitOfWork
        )
        {
            _deviceRepo = deviceRepo;
            _userRepo = userRepo;
            _departmentRepo = departmentRepo;
            _sectionRepo = sectionRepo;
            _maintenanceRepo = maintenanceRepo;
            _transferRepo = transferRepo;
            _decommissioningRepo = decommissioningRepo;
            _inspectionRepo = inspectionRepo;
            _unitOfWork = unitOfWork;
        }

        #region Queries

        public async Task<IEnumerable<DeviceDto>> GetDevicesAsync(
            int currentUserId,
            string role,
            DeviceFilterDto? filter = null
        )
        {
            var devices = await _deviceRepo.GetAllAsync();
            var filteredDevices = await FilterDevicesByRoleAsync(currentUserId, role, devices);

            if (filter != null)
            {
                filteredDevices = ApplyFilters(filteredDevices, filter);
            }

            var deviceDtos = await MapToDeviceDtosAsync(filteredDevices);

            if (filter != null)
            {
                return ApplyOrderingAndPaging(deviceDtos, filter);
            }

            return deviceDtos;
        }

        public async Task<DeviceDetailDto> GetDeviceDetailsAsync(int deviceId)
        {
            var device =
                await _deviceRepo.GetByIdAsync(deviceId)
                ?? throw new EntityNotFoundException("Device", deviceId);

            var department =
                await _departmentRepo.GetByIdAsync(device.DepartmentId)
                ?? throw new EntityNotFoundException("Department", device.DepartmentId);

            var section =
                await _sectionRepo.GetByIdAsync(department.SectionId)
                ?? throw new EntityNotFoundException("Section", department.SectionId);

            string? sectionManagerName = null;
            if (section.SectionManagerId.HasValue)
            {
                var sectionManager = await _userRepo.GetByIdAsync(section.SectionManagerId.Value);
                sectionManagerName = sectionManager?.FullName;
            }

            var maintenanceHistory = await GetMaintenanceHistoryAsync(device);
            var transferHistory = await GetTransferHistoryAsync(device);
            var decommissioning = await GetDecommissioningInfoAsync(device);

            return new DeviceDetailDto(
                device.DeviceId,
                device.Name,
                device.Type,
                device.OperationalState,
                department.Name,
                maintenanceHistory,
                transferHistory,
                decommissioning,
                device.AcquisitionDate,
                section.Name,
                sectionManagerName
            );
        }

        public async Task<IEnumerable<DeviceDto>> GetDevicesBySectionAsync(int sectionId)
        {
            var section =
                await _sectionRepo.GetByIdAsync(sectionId)
                ?? throw new EntityNotFoundException("Section", sectionId);

            var devices = await _deviceRepo.GetAllAsync();
            var departments = await LoadDepartmentsAsync(devices);

            var sectionDevices = devices
                .Where(d =>
                    departments.ContainsKey(d.DepartmentId)
                    && departments[d.DepartmentId].SectionId == sectionId
                )
                .ToList();

            return await MapToDeviceDtosAsync(sectionDevices);
        }

        public async Task<IEnumerable<DeviceDto>> GetMySectionDevicesAsync(int currentUserId)
        {
            var user =
                await _userRepo.GetByIdAsync(currentUserId)
                ?? throw new EntityNotFoundException("User", currentUserId);

            var userDepartment =
                await _departmentRepo.GetByIdAsync(user.DepartmentId)
                ?? throw new EntityNotFoundException("Department", user.DepartmentId);

            var devices = await _deviceRepo.GetAllAsync();
            var departments = await LoadDepartmentsAsync(devices);

            var sectionDevices = devices
                .Where(d =>
                    departments.ContainsKey(d.DepartmentId)
                    && departments[d.DepartmentId].SectionId == userDepartment.SectionId
                )
                .ToList();

            return await MapToDeviceDtosAsync(sectionDevices);
        }

        #endregion

        #region Commands

        public async Task<DeviceDto> RegisterDeviceAsync(RegisterDeviceDto request, int currentUserId)
        {
            var device = new Device(
                request.Name,
                request.DeviceType,
                OperationalState.UnderRevision,
                null,
                request.AcquisitionDate
            );

            await _deviceRepo.AddAsync(device);
            await _unitOfWork.SaveChangesAsync();

            var savedDevice = await _deviceRepo.GetDeviceByNameAsync(device.Name)
                ?? throw new EntityNotFoundException("Device", device.Name);

            var inspectionRequest = new Domain.Aggregations.ReceivingInspectionRequest(
                DateTime.Now,
                savedDevice.DeviceId,
                currentUserId,
                request.TechnicianId
            );

            await _inspectionRepo.AddAsync(inspectionRequest);
            await _unitOfWork.SaveChangesAsync();

            return CreateDeviceDto(savedDevice, null);
        }

        public async Task UpdateDeviceAsync(int deviceId, UpdateDeviceRequestDto request)
        {
            var existingDevice =
                await _deviceRepo.GetByIdAsync(deviceId)
                ?? throw new EntityNotFoundException("Device", deviceId);

            var department =
                await _departmentRepo.GetDepartmentByNameAsync(request.DepartmentName)
                ?? throw new EntityNotFoundException("Department", request.DepartmentName);

            var device = new Device(
                request.Name,
                request.DeviceType,
                request.OperationalState,
                department.DepartmentId,
                request.Date,
                deviceId
            );

            await _deviceRepo.UpdateAsync(device);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDeviceAsync(int deviceId)
        {
            var device =
                await _deviceRepo.GetByIdAsync(deviceId)
                ?? throw new EntityNotFoundException("Device", deviceId);

            await _deviceRepo.DeleteAsync(device);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DisableDeviceAsync(int deviceId)
        {
            var device =
                await _deviceRepo.GetByIdAsync(deviceId)
                ?? throw new EntityNotFoundException("Device", deviceId);

            device.Disable();
            await _deviceRepo.UpdateAsync(device);
            await _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Filtra dispositivos según el rol del usuario.
        /// - Administrator/Director: Todos los dispositivos
        /// - SectionManager/Technician/EquipmentReceiver/Logistician: Solo dispositivos de su sección
        /// </summary>
        private async Task<IEnumerable<Device>> FilterDevicesByRoleAsync(
            int currentUserId,
            string role,
            IEnumerable<Device> devices
        )
        {
            // Roles con acceso total
            if (role == nameof(RoleEnum.Administrator) || role == nameof(RoleEnum.Director))
            {
                return devices;
            }

            // Roles con acceso a su sección
            var user = await _userRepo.GetByIdAsync(currentUserId);
            if (user == null)
                return Enumerable.Empty<Device>();

            var userDepartment = await _departmentRepo.GetByIdAsync(user.DepartmentId);
            if (userDepartment == null)
                return Enumerable.Empty<Device>();

            var departments = await LoadDepartmentsAsync(devices);

            return devices
                .Where(d =>
                    departments.ContainsKey(d.DepartmentId)
                    && departments[d.DepartmentId].SectionId == userDepartment.SectionId
                )
                .ToList();
        }

        private static IEnumerable<Device> ApplyFilters(
            IEnumerable<Device> devices,
            DeviceFilterDto filter
        )
        {
            return devices.Where(d =>
                (!filter.DeviceType.HasValue || d.Type == filter.DeviceType)
                && (
                    !filter.OperationalState.HasValue
                    || d.OperationalState == filter.OperationalState
                )
                && (!filter.DepartmentId.HasValue || d.DepartmentId == filter.DepartmentId)
            );
        }

        private async Task<Dictionary<int, Department>> LoadDepartmentsAsync(
            IEnumerable<Device> devices
        )
        {
            var departmentIds = devices.Select(d => d.DepartmentId).Distinct();
            var departments = new Dictionary<int, Department>();

            foreach (var deptId in departmentIds)
            {
                var dept = await _departmentRepo.GetByIdAsync(deptId);
                if (dept != null)
                    departments[deptId] = dept;
            }

            return departments;
        }

        private async Task<List<DeviceDto>> MapToDeviceDtosAsync(IEnumerable<Device> devices)
        {
            var deviceList = devices.ToList();
            if (!deviceList.Any())
                return new List<DeviceDto>();

            // Cargar todos los departamentos necesarios en batch
            var departmentIds = deviceList.Select(d => d.DepartmentId).Distinct();
            var departments = new Dictionary<int, Department>();

            foreach (var deptId in departmentIds)
            {
                var dept = await _departmentRepo.GetByIdAsync(deptId);
                if (dept != null)
                    departments[deptId] = dept;
            }

            // Mapear todos los dispositivos usando el diccionario
            return deviceList
                .Select(device =>
                    CreateDeviceDto(device, departments.GetValueOrDefault(device.DepartmentId))
                )
                .ToList();
        }

        private static DeviceDto CreateDeviceDto(Device device, Department? department)
        {
            return new DeviceDto(
                device.DeviceId,
                device.Name,
                device.Type,
                device.OperationalState,
                department?.Name ?? "Unassigned"
            );
        }

        private static IEnumerable<DeviceDto> ApplyOrderingAndPaging(
            IEnumerable<DeviceDto> devices,
            DeviceFilterDto filter
        )
        {
            if (string.IsNullOrEmpty(filter.OrderBy))
            {
                return devices;
            }

            IEnumerable<DeviceDto> ordered = filter.OrderBy.ToLower() switch
            {
                "name" => devices.OrderBy(x => x.Name),
                "deviceid" => devices.OrderBy(x => x.DeviceId),
                _ => devices,
            };

            if (filter.IsDescending)
            {
                ordered = ordered.Reverse();
            }

            int skip = filter.PageNumber * filter.PageSize;
            return ordered.Skip(skip).Take(filter.PageSize);
        }

        private async Task<List<MaintenanceRecordDto>> GetMaintenanceHistoryAsync(Device device)
        {
            var maintenanceHistory = await _maintenanceRepo.GetMaintenancesByDeviceAsync(
                device.DeviceId
            );
            var maintenanceList = maintenanceHistory.ToList();

            if (!maintenanceList.Any())
                return new List<MaintenanceRecordDto>();

            // Cargar todos los técnicos necesarios en batch
            var technicianIds = maintenanceList.Select(m => m.TechnicianId).Distinct();
            var technicians = new Dictionary<int, User>();

            foreach (var techId in technicianIds)
            {
                var tech = await _userRepo.GetByIdAsync(techId);
                if (tech != null)
                    technicians[techId] = tech;
            }

            return maintenanceList
                .Select(m => new MaintenanceRecordDto(
                    m.MaintenanceRecordId,
                    m.DeviceId,
                    device.Name,
                    m.TechnicianId,
                    technicians.GetValueOrDefault(m.TechnicianId)?.FullName ?? "Unknown",
                    m.Date,
                    m.Type,
                    m.Cost,
                    m.Description
                ))
                .ToList();
        }

        private async Task<List<TransferDto>> GetTransferHistoryAsync(Device device)
        {
            var transferHistory = await _transferRepo.GetTransfersByDeviceAsync(device.DeviceId);
            var transferList = transferHistory.ToList();

            if (!transferList.Any())
                return new List<TransferDto>();

            // Cargar todos los usuarios y secciones necesarios en batch
            var receiverIds = transferList.Select(t => t.DeviceReceiverId).Distinct();
            var sectionIds = transferList
                .SelectMany(t => new[] { t.SourceSectionId, t.DestinationSectionId })
                .Distinct();

            var receivers = new Dictionary<int, User>();
            var sections = new Dictionary<int, Domain.Entities.Section>();

            foreach (var receiverId in receiverIds)
            {
                var receiver = await _userRepo.GetByIdAsync(receiverId);
                if (receiver != null)
                    receivers[receiverId] = receiver;
            }

            foreach (var sectionId in sectionIds)
            {
                var section = await _sectionRepo.GetByIdAsync(sectionId);
                if (section != null)
                    sections[sectionId] = section;
            }

            return transferList
                .Select(t => new TransferDto(
                    t.TransferId,
                    t.DeviceId,
                    device.Name,
                    t.Date,
                    t.SourceSectionId,
                    sections.GetValueOrDefault(t.SourceSectionId)?.Name ?? "Unknown",
                    t.DestinationSectionId,
                    sections.GetValueOrDefault(t.DestinationSectionId)?.Name ?? "Unknown",
                    t.DeviceReceiverId,
                    receivers.GetValueOrDefault(t.DeviceReceiverId)?.FullName ?? "Unknown",
                    t.Status
                ))
                .ToList();
        }

        private async Task<DecommissioningDto?> GetDecommissioningInfoAsync(Device device)
        {
            var decommissioning = (
                await _decommissioningRepo.GetDecommissioningRequestsByDeviceAsync(device.DeviceId)
            )
                .Where(d => d.IsApproved)
                .FirstOrDefault();

            if (decommissioning == null)
            {
                return null;
            }

            var receiver = await _userRepo.GetByIdAsync((int)decommissioning.DeviceReceiverId!);
            var receiverDepartment = await _departmentRepo.GetByIdAsync(
                (int)decommissioning.DeviceReceiverId!
            );

            return new DecommissioningDto
            {
                DeviceId = decommissioning.DeviceId,
                DeviceName = device.Name,
                DecommissioningRequestId = decommissioning.DecommissioningRequestId,
                DeviceReceiverId = (int)decommissioning.DeviceReceiverId,
                DeviceReceiverName = receiver?.FullName ?? "Unknown",
                ReceiverDepartmentId = receiverDepartment?.DepartmentId ?? 0,
                ReceiverDepartmentName = receiverDepartment?.Name ?? "Unknown",
                DecommissioningDate = decommissioning.Date,
                Reason = decommissioning.Reason,
                FinalDestination = null,
            };
        }

        #endregion
    }
}
