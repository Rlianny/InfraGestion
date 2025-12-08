using Application.DTOs.Decommissioning;
using Application.DTOs.Inventory;
using Application.DTOs.Maintenance;
using Application.DTOs.Transfer;
using Application.Services.Interfaces;
using Domain.Aggregations;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

public class InventoryService : IInventoryService
{
    private readonly IDeviceRepository deviceRepo;
    private readonly ISectionRepository sectionRepo;
    private readonly IUserRepository userRepo;
    private readonly IReceivingInspectionRequestRepository receivingInspectionRequestRepo;
    private readonly IUnitOfWork unitOfWork;
    private readonly IDepartmentRepository departmentRepository;
    private readonly IMaintenanceRecordRepository maintenanceRepo;
    private readonly ITransferRepository transferRepo;
    private readonly IDecommissioningRepository decommissioningRepository;

    public InventoryService(
        IDeviceRepository deviceRepo,
        IUserRepository userRepo,
        ISectionRepository sectionRepo,
        IReceivingInspectionRequestRepository receivingInspectionRequestRepo,
        IUnitOfWork unitOfWork,
        IDepartmentRepository departmentRepository,
        IDecommissioningRepository decommissioningRepository,
        ITransferRepository transferRepository,
        IMaintenanceRecordRepository maintenanceRepository
        )
    {
        this.deviceRepo = deviceRepo;
        this.sectionRepo = sectionRepo;
        this.userRepo = userRepo;
        this.unitOfWork = unitOfWork;
        this.departmentRepository = departmentRepository;
        this.decommissioningRepository = decommissioningRepository;
        this.receivingInspectionRequestRepo = receivingInspectionRequestRepo;
        this.transferRepo = transferRepository;
        this.maintenanceRepo = maintenanceRepository;
    }

    public async Task ProcessInspectionDecisionAsync(InspectionDecisionRequestDto request)
    {
        try
        {
            ReceivingInspectionRequest inspectionRequest = await receivingInspectionRequestRepo.GetReceivingInspectionRequestsByDeviceAsync(request.DeviceId);

            if (request.TechnicianId != inspectionRequest.TechnicianId)
            {
                throw new Exception($"Technician with id {request.TechnicianId} is not allowed to make this request");
            }

            var device = await deviceRepo.GetByIdAsync(request.DeviceId)
                ?? throw new Exception($"Device with id {request.DeviceId} not found");

            if (request.IsApproved)
            {
                inspectionRequest.Accept();
                device.UpdateOperationalState(Domain.Enums.OperationalState.Revised);
                await deviceRepo.UpdateAsync(device);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.RejectionReason))
                {
                    throw new Exception("Rejection reason is required when rejecting a device");
                }
                inspectionRequest.Reject(request.RejectionReason);
            }

            await receivingInspectionRequestRepo.UpdateAsync(inspectionRequest);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while processing inspection decision: {ex.Message}");
        }
    }

    public async Task AssignDeviceForReviewAsync(AssignDeviceForInspectionRequestDto inspectionRequestDto)
    {
        try
        {
            // Obtener el dispositivo y cambiar su estado a UnderRevision
            var device = await deviceRepo.GetByIdAsync(inspectionRequestDto.DeviceId)
                ?? throw new Exception($"Device with id {inspectionRequestDto.DeviceId} not found");

            device.UpdateOperationalState(Domain.Enums.OperationalState.UnderRevision);
            await deviceRepo.UpdateAsync(device);

            await receivingInspectionRequestRepo.AddAsync(new ReceivingInspectionRequest(DateTime.Now, inspectionRequestDto.DeviceId, inspectionRequestDto.AdministratorId, inspectionRequestDto.TechnicianId));
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to assign device for review: {ex.Message}");
        }
    }

    public async Task<IEnumerable<DeviceDto>> GetCompanyInventoryAsync()
    {
        try
        {
            var devices = await deviceRepo.GetAllAsync();
            return await Task.WhenAll(
                devices.Select(async device =>
                {
                    var department = await departmentRepository.GetByIdAsync(device.DepartmentId);
                    return new DeviceDto(device.DeviceId, device.Name, device.Type, device.OperationalState, department.Name);
                })
            );
        }
        catch
        {
            throw new Exception("Error while tryng to get company inventory");
        }
    }

    public async Task<DeviceDetailDto> GetDeviceDetailAsync(int deviceID)
    {
        try
        {
            var device = await deviceRepo.GetByIdAsync(deviceID) ?? throw new Exception("Device not found");
            var department = await departmentRepository.GetByIdAsync(device.DepartmentId) ?? throw new Exception("Department not found");
            var maintenanceHistory = await maintenanceRepo.GetMaintenancesByDeviceAsync(device.DeviceId);

            var maintenanceDtos = new List<MaintenanceRecordDto>();
            foreach (var m in maintenanceHistory)
            {
                var technician = await userRepo.GetByIdAsync(m.TechnicianId);
                maintenanceDtos.Add(new MaintenanceRecordDto(
                    m.MaintenanceRecordId,
                    m.DeviceId,
                    device.Name,
                    m.TechnicianId,
                    technician?.FullName ?? "Unknown",
                    m.Date,
                    m.Type,
                    m.Cost,
                    m.Description
                ));
            }

            var transferHistory = await transferRepo.GetTransfersByDeviceAsync(device.DeviceId);
            var transferDtos = new List<TransferDto>();
            foreach (var t in transferHistory)
            {
                var receiver = await userRepo.GetByIdAsync(t.DeviceReceiverId);
                var sourceSection = await sectionRepo.GetByIdAsync(t.SourceSectionId);
                var destSection = await sectionRepo.GetByIdAsync(t.DestinationSectionId);
                transferDtos.Add(new TransferDto(
                    t.TransferId,
                    t.DeviceId,
                    device.Name,
                    t.Date,
                    t.SourceSectionId,
                    sourceSection?.Name ?? "Unknown",
                    t.DestinationSectionId,
                    destSection?.Name ?? "Unknown",
                    t.DeviceReceiverId,
                    receiver?.FullName ?? "Unknown",
                    t.Status
                ));
            }

            var decommissioning = await decommissioningRepository.GetDecommissioningByDeviceAsync(device.DeviceId);
            DecommissioningDto? decommissioningDto = null;
            if (decommissioning != null)
            {
                var decommReceiver = await userRepo.GetByIdAsync(decommissioning.DeviceReceiverId);
                var receiverDepartment = await departmentRepository.GetByIdAsync(decommissioning.ReceiverDepartmentId);
                decommissioningDto = new DecommissioningDto
                {
                    DecommissioningId = decommissioning.DecommissioningId,
                    DeviceId = decommissioning.DeviceId,
                    DeviceName = device.Name,
                    DecommissioningRequestId = decommissioning.DecommissioningRequestId,
                    DeviceReceiverId = decommissioning.DeviceReceiverId,
                    DeviceReceiverName = decommReceiver?.FullName ?? "Unknown",
                    ReceiverDepartmentId = decommissioning.ReceiverDepartmentId,
                    ReceiverDepartmentName = receiverDepartment?.Name ?? "Unknown",
                    DecommissioningDate = decommissioning.DecommissioningDate,
                    Reason = decommissioning.Reason,
                    FinalDestination = decommissioning.FinalDestination
                };
            }

            return new DeviceDetailDto(
                device.DeviceId,
                device.Name,
                device.Type,
                device.OperationalState,
                department.Name,
                maintenanceDtos,
                transferDtos,
                decommissioningDto,
                device.AcquisitionDate
            );
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to get device detail: {ex.Message}");
        }
    }

    public async Task<IEnumerable<DeviceDto>> GetInventoryAsync(DeviceFilterDto filter, int userId)
    {
        try
        {
            var devices = await deviceRepo.GetAllAsync();
            var filtered = devices.Where(d =>
                (!filter.DeviceType.HasValue || d.Type == filter.DeviceType) &&
                (!filter.OperationalState.HasValue || d.OperationalState == filter.OperationalState) &&
                (!filter.DepartmentId.HasValue || d.DepartmentId == filter.DepartmentId)
            );

            var deviceDtos = new List<DeviceDto>();
            foreach (var device in filtered)
            {
                var department = await departmentRepository.GetByIdAsync(device.DepartmentId);
                deviceDtos.Add(new DeviceDto(device.DeviceId, device.Name, device.Type, device.OperationalState, department.Name));
            }

            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                IEnumerable<DeviceDto> ordered = filter.OrderBy.ToLower() switch
                {
                    "name" => deviceDtos.OrderBy(x => x.Name),
                    "deviceid" => deviceDtos.OrderBy(x => x.DeviceId),
                    _ => throw new NotImplementedException()
                };
                int skip = filter.PageNumber * filter.PageSize;
                if (filter.IsDescending)
                {
                    ordered = ordered.Reverse();
                }
                return ordered.Skip(skip).Take(filter.PageSize);
            }

            return deviceDtos;
        }
        catch
        {
            throw new Exception("Error while tryng to get inventory devices");
        }
    }

    public async Task<IEnumerable<DeviceDto>> GetSectionInventoryAsync(int userID, int sectionId)
    {
        try
        {
            var user = await userRepo.GetByIdAsync(userID) ?? throw new Exception("User not found");
            var devices = await deviceRepo.GetAllAsync();
            var deviceDtos = new List<DeviceDto>();
            foreach (var device in devices)
            {
                var department = await departmentRepository.GetByIdAsync(device.DepartmentId);
                if (department!.SectionId == sectionId && user.DepartmentId == department.DepartmentId)
                    deviceDtos.Add(new DeviceDto(device.DeviceId, device.Name, device.Type, device.OperationalState, department.Name));
            }
            return deviceDtos;
        }
        catch
        {
            throw new Exception("Error while tryng to get own section inventory");
        }
    }

    public async Task<IEnumerable<DeviceDto>> GetUsersOwnSectionInventory(int userID)
    {
        try
        {
            var user = await userRepo.GetByIdAsync(userID) ?? throw new Exception("User does not exist");
            var userDepartment = await departmentRepository.GetByIdAsync(user.DepartmentId) ?? throw new Exception("The given user does not exist");
            var devices = await deviceRepo.GetAllAsync();
            var deviceDtos = new List<DeviceDto>();
            foreach (var device in devices)
            {
                var deviceDepartment = await departmentRepository.GetByIdAsync(device.DepartmentId) ?? throw new Exception("User is not on that departament exist");
                if (userDepartment.SectionId == deviceDepartment.SectionId)
                {
                    deviceDtos.Add(new DeviceDto(device.DeviceId, device.Name, device.Type, device.OperationalState, deviceDepartment.Name));
                }
            }
            return deviceDtos;
        }
        catch
        {
            throw new Exception("Error while tryng to get own section inventory");
        }
    }

    public async Task RegisterDeviceAsync(InsertDeviceRequestDto request)
    {
        try
        {
            var device = new Device(
                request.Name,
                request.DeviceType,
                Domain.Enums.OperationalState.UnderRevision,
                null,
                DateTime.Now
            );

            await deviceRepo.AddAsync(device);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error tryng to register device async");
        }
    }



    public async Task UpdateEquipmentAsync(UpdateDeviceRequestDto request)
    {
        try
        {
            var department = await departmentRepository.GetDepartmentByNameAsync(request.DepartmentName) ?? throw new Exception($"Deparment with name{request.DepartmentName} does not exists");
            Device device = new Device(request.Name, request.DeviceType, request.OperationalState, department.DepartmentId, request.Date, request.DeviceId);
            await deviceRepo.UpdateAsync(device);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error while trying to update device");
        }
    }
    public async Task DeleteEquimentAsync(DeleteDeviceRequestDto deleteRequest)
    {
        try
        {
            var device = await deviceRepo.GetByIdAsync(deleteRequest.DeviceId) ?? throw new Exception();
            await deviceRepo.DeleteAsync(device);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error ocurred while tryng to delete the device");
        }
    }
}