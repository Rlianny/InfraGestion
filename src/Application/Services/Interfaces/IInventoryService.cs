using Application.DTOs.Inventory;
using Domain.Entities;

public interface IInventoryService
{
    Task RegisterDeviceAsync(InsertDeviceRequestDto request);
    Task<IEnumerable<DeviceDto>> GetInventoryAsync(DeviceFilterDto filter, int UserId);
    Task<DeviceDetailDto> GetDeviceDetailAsync(int DeviceId);
    Task UpdateEquipmentAsync(UpdateDeviceRequestDto request);
    Task<IEnumerable<DeviceDto>> GetUsersOwnSectionInventory(int userId);
    Task<IEnumerable<DeviceDto>> GetSectionInventoryAsync(int userId, int sectionId);
    Task<IEnumerable<DeviceDto>> GetCompanyInventoryAsync();
    Task AssignDeviceForReviewAsync(AssignDeviceForInspectionRequestDto inspectionRequestDto);
    Task RejectDevice(int deviceId, int technicianId, string reason);
    Task ApproveDevice(int deviceId, int technicianId);
    Task DeleteEquimentAsync(DeleteDeviceRequestDto deleteRequest);
}