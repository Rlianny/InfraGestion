using Application.DTOs.Inventory;
using Domain.Entities;

public interface IInventoryService
{
    Task RegisterDeviceAsync(InsertDeviceRequestDto request);
    Task<IEnumerable<DeviceDto>> GetInventoryAsync(DeviceFilterDto filter, int UserId);
    Task<DeviceDetailDto> GetDeviceDetailAsync(int DeviceID);
    Task UpdateEquipmentAsync(UpdateDeviceRequestDto request);
    Task<IEnumerable<DeviceDto>> GetUsersOwnSectionInventory(int userID);
    Task<IEnumerable<DeviceDto>> GetSectionInventoryAsync(int userID, int sectionId);
    Task<IEnumerable<DeviceDto>> GetCompanyInventoryAsync(int userID);
    Task AssignDeviceForReviewAsync(int deviceID, int technicianID);
    Task RejectDevice(int deviceID, int technicianID, string reason);
    Task ApproveDevice(int deviceID, int technicianID);
}