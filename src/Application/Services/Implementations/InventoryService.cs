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

    public async Task ApproveDevice(int deviceID, int technicianID)
    {
        try
        {
            ReceivingInspectionRequest inspectionRequest = await receivingInspectionRequestRepo.GetReceivingInspectionRequestsByDeviceAsync(deviceID);
            if (technicianID != inspectionRequest.TechnicianId)
            {
                throw new Exception($"Thechichian with id {technicianID} its not allowed to make this request");
            }

            inspectionRequest.Accept();
            await receivingInspectionRequestRepo.UpdateAsync(inspectionRequest);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error while trying to approve the device");
        }
    }

    public async Task AssignDeviceForReviewAsync(AssignDeviceForInspectionRequestDto inspectionRequestDto)
    {
        try
        {
            await receivingInspectionRequestRepo.AddAsync(new ReceivingInspectionRequest(DateTime.Now, inspectionRequestDto.DeviceId, inspectionRequestDto.AdministratorId, inspectionRequestDto.TechnicianId));
        }
        catch (Exception ex)
        {
            throw new Exception("Error while tryng to assign device for review");
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

            //var maintenanceHistory = await maintenanceRepo.GetMaintenancesByDeviceAsync(device.DeviceId);
            
            //TODO
            //Fixing this
            //var transferHistory = await transferRepo.GetTransfersByDeviceAsync(device.DeviceId);
            //var decommissionings = await decommissioningRepository.GetDecommissioningsByDeviceAsync(device.DeviceId);
            

            var maintenanceDtos = new List<MaintenanceRecordDto>();
           /* await Task.WhenAll(maintenanceHistory.Select(async m =>
            {
                //var technician = await userRepo.GetByIdAsync(m.TechnicianId);
                return new MaintenanceRecordDto(
                    m.MaintenanceRecordId,
                    m.DeviceId,
                    device.Name,
                    m.TechnicianId,
                    "Unknown",//technician?.FullName ?? 
                    m.Date,
                    m.Type,
                    m.Cost,
                    m.Description
                );
            }));
           */
            var transferDtos = new List<TransferDto>();
            /* await Task.WhenAll(transferHistory.Select(async t =>
             {
                 var receiver = await userRepo.GetByIdAsync(t.DeviceReceiverId);
                 return new TransferDto
                 {
                     DeviceId = t.DeviceId,
                     DestinationSectionId = t.DestinationSectionId,
                     DeviceReceiverId = t.DeviceReceiverId,
                     DeviceReceiverName = receiver?.FullName ?? "Unknown"
                 };
             })); */


            //decommissionings.FirstOrDefault(x => x.FinalDestination != null);
            DecommissioningDto? decommissioningDto = null;
            /*finalDecommissioning != null
                ? new DecommissioningDto(
                    finalDecommissioning.DecommissioningId,
                    finalDecommissioning.DeviceReceiverId,
                    finalDecommissioning.DecommissioningRequestId,
                    finalDecommissioning.DeviceId,
                    finalDecommissioning.DecommissioningDate,
                    finalDecommissioning.Reason,
                    finalDecommissioning.FinalDestination,
                    finalDecommissioning.ReceiverDepartmentId
                )
                : null;
            */
            return new DeviceDetailDto(
                device.DeviceId,
                device.Name,
                device.Type,
                device.OperationalState,
                department.Name,
                maintenanceDtos,
                transferDtos,
                decommissioningDto
            );
        }
        catch
        {
            throw new Exception("Error while trying to get device detail");
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
                var deviceDepartment = await departmentRepository.GetByIdAsync(user.DepartmentId) ?? throw new Exception("User is not on that departament exist");
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
                Domain.Enums.OperationalState.Operational,
                request.DepartmentId,
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

    public async Task RejectDevice(int deviceID, int technicianID, string reason)
    {
        try
        {
            ReceivingInspectionRequest inspectionRequest = await receivingInspectionRequestRepo.GetReceivingInspectionRequestsByDeviceAsync(deviceID);
            User? tech = await userRepo.GetByIdAsync(technicianID);
            Device? device = await deviceRepo.GetByIdAsync(deviceID);
            if (tech == null || device == null || !tech.IsTechnician || technicianID != inspectionRequest.TechnicianId)
            {
                throw new Exception();
            }
            inspectionRequest.Reject(reason);
            await receivingInspectionRequestRepo.UpdateAsync(inspectionRequest);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error while tryng to reject device");
        }
    }

    public async Task UpdateEquipmentAsync(UpdateDeviceRequestDto request)
    {
        try
        {
            Device device = new Device(request.Name, request.DeviceType, request.OperationalState, request.DepartmentId, DateTime.Now, request.DeviceId);
            await deviceRepo.UpdateAsync(device);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error while tryng to update device");
        }
    }
}