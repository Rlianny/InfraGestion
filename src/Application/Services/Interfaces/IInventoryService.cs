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
    Task<IEnumerable<DeviceDto>> GetCompanyInventoryAsync(int userId);
    Task AssignDeviceForReviewAsync(AssignDeviceForInspectionRequestDto inspectionRequestDto);
    Task ProcessInspectionDecisionAsync(InspectionDecisionRequestDto request);
    Task DisableEquipmentAsync(int id);
    Task DeleteEquipmentAsync(int id);
    Task<IEnumerable<ReceivingInspectionRequestDto>> ReceivingInspectionRequestsByTechnician(int technicianId);
    Task<IEnumerable<ReceivingInspectionRequestDto>> GetReceivingInspectionRequestsByTechnicianAsync(int technicianId);
    Task<IEnumerable<ReceivingInspectionRequestDto>> GetReceivingInspectionRequestsByAdminAsync(int adminId);
    Task<IEnumerable<ReceivingInspectionRequestDto>> GetPendingReceivingInspectionRequestByTechnicianAsync(int technicianId);
}