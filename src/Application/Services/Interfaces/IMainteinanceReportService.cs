public interface IMaintenanceReportService
{
    Task<MaintenanceReportDto> GenerateMaintenanceReportAsync(MaintenanceReportCriteria criteria);
    Task<List<EquipmentReplacementAlertDto>> GetEquipmentReplacementAlertsAsync();
}