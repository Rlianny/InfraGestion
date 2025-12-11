using Application.DTOs;
using Application.DTOs.Decommissioning;
using Application.DTOs.Inventory;
using Application.DTOs.Maintenance;
using Application.DTOs.Transfer;
using Application.Services.Interfaces;
using Domain.Aggregations;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
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
    private readonly IDecommissioningRequestRepository _requestRepository;

    public InventoryService(
        IDeviceRepository deviceRepo,
        IUserRepository userRepo,
        ISectionRepository sectionRepo,
        IReceivingInspectionRequestRepository receivingInspectionRequestRepo,
        IUnitOfWork unitOfWork,
        IDepartmentRepository departmentRepository,
        IDecommissioningRequestRepository decommissioningRequestRepository,
        ITransferRepository transferRepository,
        IMaintenanceRecordRepository maintenanceRepository
        )
    {
        this.deviceRepo = deviceRepo;
        this.sectionRepo = sectionRepo;
        this.userRepo = userRepo;
        this.unitOfWork = unitOfWork;
        this.departmentRepository = departmentRepository;
        this._requestRepository = decommissioningRequestRepository;
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

            device.UpdateOperationalState(Domain.Enums.OperationalState.Revised);
            await deviceRepo.UpdateAsync(device);
            if (request.IsApproved)
            {
                inspectionRequest.Accept();
            }
            else
            {

                inspectionRequest.Reject(request.Reason);

                // Create a decommissioning request addressed to the section manager of the device's section
                var department = await departmentRepository.GetByIdAsync(device.DepartmentId)
                    ?? throw new Exception($"Department with id {device.DepartmentId} not found for device {device.DeviceId}");

                var section = await sectionRepo.GetByIdAsync(department.SectionId)
                    ?? throw new Exception($"Section with id {department.SectionId} not found for department {department.DepartmentId}");

                if (!section.SectionManagerId.HasValue)
                {
                    throw new Exception($"Section {section.SectionId} does not have an assigned manager to receive the decommissioning request");
                }

                var decommissioningRequest = new DecommissioningRequest(
                    request.TechnicianId,
                    request.DeviceId,
                    DateTime.Now,
                    request.Reason
                );

                await _requestRepository.AddAsync(decommissioningRequest);
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

    public async Task<IEnumerable<DeviceDto>> GetCompanyInventoryAsync(int userID)
    {
        try
        {
            var user = await userRepo.GetByIdAsync(userID);
            var devices = await deviceRepo.GetAllAsync();
            IEnumerable<Device> finalDevices = [];
            if (user.IsTechnician || user.IsSectionManager)
            {
                var dep = await departmentRepository.GetByIdAsync(user.DepartmentId);
                System.Console.WriteLine(dep.SectionId);
                foreach (var d in devices)
                {
                    var depart = await departmentRepository.GetByIdAsync(d.DepartmentId);
                    if (depart.SectionId == dep.SectionId)
                    {
                        System.Console.WriteLine("Add device " + d.Name);
                        finalDevices = finalDevices.Append(d);
                    }
                }

            }

            else if (user.IsAdministrator)
            {
                finalDevices = devices;
            }

            return await Task.WhenAll(
                finalDevices.Select(async device =>
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
            System.Console.WriteLine(device.Name);
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
            var decommissioning = (await _requestRepository.GetDecommissioningRequestsByDeviceAsync(device.DeviceId)).Where(d => d.IsApproved).FirstOrDefault();
            DecommissioningDto? decommissioningDto = null;
            if (decommissioning != null)
            {
                var decommReceiver = await userRepo.GetByIdAsync(decommissioning.DeviceReceiverId);
                var receiverDepartment = await departmentRepository.GetByIdAsync(decommissioning.DeviceReceiverId);
                decommissioningDto = new DecommissioningDto
                {
                    DeviceId = decommissioning.DeviceId,
                    DeviceName = device.Name,
                    DecommissioningRequestId = decommissioning.DecommissioningRequestId,
                    DeviceReceiverId = decommissioning.DeviceReceiverId,
                    DeviceReceiverName = decommReceiver?.FullName ?? "Unknown",
                    ReceiverDepartmentId = decommissioning.DeviceReceiverId,
                    ReceiverDepartmentName = receiverDepartment?.Name ?? "Unknown",
                    DecommissioningDate = decommissioning.Date,
                    Reason = decommissioning.Reason,
                    FinalDestination = null
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
        var device = new Device(
            request.Name,
            request.DeviceType,
            Domain.Enums.OperationalState.UnderRevision,
            null,
            request.AcquisitionDate
        );

        await deviceRepo.AddAsync(device);
        await unitOfWork.SaveChangesAsync();
        var dev = await deviceRepo.GetDeviceByNameAsync(device.Name);
        var receivingInspectionRequest = new ReceivingInspectionRequest(DateTime.Now, dev.DeviceId, request.userID, request.technicianId);
        await receivingInspectionRequestRepo.AddAsync(receivingInspectionRequest);
        await unitOfWork.SaveChangesAsync();
    }
    public async Task<IEnumerable<ReceivingInspectionRequestDto>> ReceivingInspectionRequestsByTechnician(int technicianId)
    {
        var inspections = (await receivingInspectionRequestRepo.GetAllAsync()).Where(t => t.TechnicianId == technicianId);
        List<ReceivingInspectionRequestDto> receivingInspectionRequests = [];
        foreach (var inspection in inspections)
        {
            receivingInspectionRequests.Add(new ReceivingInspectionRequestDto(inspection.ReceivingInspectionRequestId,
                                                                              inspection.EmissionDate, inspection.DeviceId,
                                                                               inspection.AdministratorId, inspection.TechnicianId,
                                                                                inspection.Status, inspection.RejectReason));
        }
        return receivingInspectionRequests;
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
    public async Task DeleteEquipmentAsync(int id)
    {
        try
        {
            var device = await deviceRepo.GetByIdAsync(id) ?? throw new EntityNotFoundException("Devices", id);
            await deviceRepo.DeleteAsync(device);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error ocurred while tryng to delete the device");
        }
    }
    public async Task DisableEquipmentAsync(int id)
    {
        try
        {
            var device = await deviceRepo.GetByIdAsync(id) ?? throw new EntityNotFoundException("Devices", id);
            device.Disable();
            await deviceRepo.UpdateAsync(device);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error ocurred while tryng to delete the device");
        }

    }
    private ReceivingInspectionRequestDto MapReceivingToReceivingDto(ReceivingInspectionRequest receivingInspectionRequest)
    {
        return new ReceivingInspectionRequestDto(receivingInspectionRequest.ReceivingInspectionRequestId,
        receivingInspectionRequest.EmissionDate,
        receivingInspectionRequest.DeviceId,
         receivingInspectionRequest.AdministratorId,
         receivingInspectionRequest.TechnicianId,
         receivingInspectionRequest.Status,
         receivingInspectionRequest.RejectReason);
    }
    public async Task<IEnumerable<ReceivingInspectionRequestDto>> GetReceivingInspectionRequestsByTechnicianAsync(int technicianId)
    {
        return (await receivingInspectionRequestRepo.GetReceivingInspectionRequestsByTechnicianAsync(technicianId)).Select(MapReceivingToReceivingDto);
    }

    public async Task<IEnumerable<ReceivingInspectionRequestDto>> GetReceivingInspectionRequestsByAdminAsync(int adminId)
    {
        return (await receivingInspectionRequestRepo.GetReceivingInspectionRequestsByAdministratorAsync(adminId)).Select(MapReceivingToReceivingDto);
    }
    public async Task<IEnumerable<ReceivingInspectionRequestDto>> GetPendingReceivingInspectionRequestByTechnicianAsync(int technicianId)
    {
        return (await receivingInspectionRequestRepo.GetReceivingInspectionRequestsByTechnicianAsync(technicianId)).Where(p => p.Status == InspectionRequestStatus.Pending).Select(MapReceivingToReceivingDto);
    }

    public async Task<IEnumerable<DeviceDto>> GetRevisedDevicesByAdmin(int adminId)
    {
        var insp = (await receivingInspectionRequestRepo.GetReceivingInspectionRequestsByAdministratorAsync(adminId)).Where(r => r.IsPending());
        List<DeviceDto> devices = [];
        foreach (var item in insp)
        {
            var device = await deviceRepo.GetByIdAsync(item.DeviceId);
            var dpt = await departmentRepository.GetByIdAsync(device.DepartmentId);
            devices.Add(new DeviceDto(device.DeviceId, device.Name, device.Type, device.OperationalState, dpt.Name));
        }
        return devices;
    }
}