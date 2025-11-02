public interface ITechnicianReportService
{
    Task<TechnicianPerformanceReportDto> GeneratePerformanceReportAsync(string technicianId, DateRange dateRange);
    Task<TechnicianEffectivenessReportDto> GenerateEffectivenessReportAsync(TechnicianReportCriteria criteria);
}