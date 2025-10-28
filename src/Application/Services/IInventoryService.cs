public interface IInventoryService
{
    Task<EquipmentDto> RegisterEquipmentAsync(RegisterEquipmentRequest request);
    Task<EquipmentDto> UpdateEquipmentAsync(string equipmentId, UpdateEquipmentRequest request);
    Task<List<EquipmentDto>> GetInventoryAsync(InventoryFilter filter);
    Task<EquipmentDetailDto> GetEquipmentDetailAsync(string equipmentId);
    Task AssignEquipmentForReviewAsync(string equipmentId, string technicianId);
    Task<EquipmentReviewResult> ReviewEquipmentAsync(string equipmentId, EquipmentReviewRequest request);
    Task<List<EquipmentDto>> GetSectionInventoryAsync(string sectionId);
    Task<List<EquipmentDto>> GetCompanyInventoryAsync();
}