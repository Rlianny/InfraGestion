public interface IMaintenanceService
{
    Task<MaintenanceRecordDto> RegisterMaintenanceAsync(RegisterMaintenanceRequest request);
    Task<List<MaintenanceRecordDto>> GetEquipmentMaintenanceHistoryAsync(string equipmentId);
    Task<List<MaintenanceRecordDto>> GetTechnicianMaintenanceHistoryAsync(string technicianId, DateRange dateRange);
    Task<MaintenanceRecordDto> GetMaintenanceRecordAsync(string maintenanceId);
}
