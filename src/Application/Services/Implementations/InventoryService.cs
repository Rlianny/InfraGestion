using Application.DTOs.Inventory;
using Domain.Entities;
using Domain.Interfaces;
using AutoMapper;
using Domain.Aggregations;

public class InventoryService : IInventoryService
{
    private readonly IRepository<Device> deviceRepo;
    private readonly IRepository<Section> sectionRepo;
    private readonly IRepository<User> userRepo;
    private readonly IReceivingInspectionRequestRepository receivingInspectionRequestRepo;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public InventoryService(
        IRepository<Device> deviceRepo,
        IRepository<User> userRepo,
        IRepository<Section> sectionRepo,
        IReceivingInspectionRequestRepository receivingInspectionRequestRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        this.deviceRepo = deviceRepo;
        this.sectionRepo = sectionRepo;
        this.userRepo = userRepo;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.receivingInspectionRequestRepo = receivingInspectionRequestRepo;
    }

    public async Task ApproveDevice(int deviceID, int technicianID)
    {
        ReceivingInspectionRequest inspectionRequest = await receivingInspectionRequestRepo.GetReceivingInspectionRequestsByDeviceAsync(deviceID);
        if (technicianID != inspectionRequest.TechnicianID)
        {
            throw new Exception();
        }

        inspectionRequest.Accept();
        await receivingInspectionRequestRepo.UpdateAsync(inspectionRequest);
    }

    public async Task AssignDeviceForReviewAsync(AssignDeviceForInspectionRequestDto inspectionRequestDto)
    {
        await receivingInspectionRequestRepo.AddAsync(new ReceivingInspectionRequest(DateTime.Now, inspectionRequestDto.DeviceId, inspectionRequestDto.AdministratorID, inspectionRequestDto.TechnicianId));
    }

    public async Task<IEnumerable<DeviceDto>> GetCompanyInventoryAsync()
    {
        var devices = await deviceRepo.GetAllAsync();
        return devices.Select(x => new DeviceDto());
    }

    public Task<DeviceDetailDto> GetDeviceDetailAsync(int DeviceID)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DeviceDto>> GetInventoryAsync(DeviceFilterDto filter, int UserId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DeviceDto>> GetSectionInventoryAsync(int userID, int sectionId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DeviceDto>> GetUsersOwnSectionInventory(int userID)
    {
        throw new NotImplementedException();
    }

    public async Task RegisterDeviceAsync(InsertDeviceRequestDto request)
    {
        await deviceRepo.AddAsync(new Device(request.Name, request.DeviceType, Domain.Enums.OperationalState.Operational, request.DepartmentID, DateTime.Now));
    }

    public async Task RejectDevice(int deviceID, int technicianID, string reason)
    {
        ReceivingInspectionRequest inspectionRequest = await receivingInspectionRequestRepo.GetReceivingInspectionRequestsByDeviceAsync(deviceID);
        if (technicianID != inspectionRequest.TechnicianID)
        {
            throw new Exception();
        }

        inspectionRequest.Reject(reason);
        await receivingInspectionRequestRepo.UpdateAsync(inspectionRequest);
    }

    public async Task UpdateEquipmentAsync(UpdateDeviceRequestDto request)
    {
        Device device = new Device(request.Name, request.DeviceType, request.OperationalState, request.DepartmentId, DateTime.Now, request.DeviceId);
        await deviceRepo.UpdateAsync(device);
    }
}