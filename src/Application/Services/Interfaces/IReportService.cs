using Application.DTOs.Report;

namespace Application.Services.Interfaces
{
    public interface IReportService
    {
        
        Task<Report<DecommissioningReportDto>> GenerateDischargeReportAsync();
        Task<Report<PersonnelEffectivenessReportDto>> GeneratePersonnelEffectivenessReportAsync(PersonnelReportFilterDto criteria);
        Task<Report<DeviceReplacementReportDto>> GenerateEquipmentReplacementReportAsync();
        Task<Report<SectionTransferReportDto>> GenerateTransferReportAsync();
        Task<Report<CorrelationAnalysisReportDto>> GenerateCorrelationAnalysisReportAsync();
        Task<Report<BonusDeterminationReportDto>> GenerateBonusDeterminationReportAsync();
        Task<Report<DeviceMantainenceReportDto>> GenerateDeviceMantainanceReportAsync(int deviceId);
    }
}