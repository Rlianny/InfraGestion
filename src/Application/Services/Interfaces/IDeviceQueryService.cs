public interface IEquipmentQueryService
{
    Task<List<EquipmentDto>> SearchEquipmentAsync(EquipmentSearchCriteria criteria);
    Task<List<EquipmentDto>> FilterEquipmentAsync(EquipmentFilter filter);
    Task<List<EquipmentDto>> GetEquipmentByStatusAsync(EquipmentStatus status);
    Task<List<EquipmentDto>> GetEquipmentByTypeAsync(string equipmentType);
}