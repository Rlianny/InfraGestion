using Application.DTOs.Report;

namespace Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<DeviceReportDto>> GenerateInventoryReportAsync(DeviceReportFilterDto filter);
        Task<IEnumerable<DecommissioningReportDto>> GenerateDischargeReportAsync(DecommissioningReportFilterDto filter);
        Task<IEnumerable<PersonnelEffectivenessReportDto>> GeneratePersonnelEffectivenessReportAsync(PersonnelReportFilterDto criteria);
        Task<IEnumerable<DeviceReplacementReportDto>> GenerateEquipmentReplacementReportAsync();
        Task<IEnumerable<SectionTransferReportDto>> GenerateTransferReportAsync();
        Task<IEnumerable<CorrelationAnalysisReportDto>> GenerateCorrelationAnalysisReportAsync();
        Task<IEnumerable<BonusDeterminationReportDto>> GenerateBonusDeterminationReportAsync(BonusReportCriteria criteria);
        Task<PdfExportDto> GeneratePdfReport(string reportType);
    }
}