public interface IReportService
{
    Task<InventoryReportDto> GenerateInventoryReportAsync(InventoryReportCriteria criteria);
    Task<DischargeReportDto> GenerateDischargeReportAsync(DischargeReportCriteria criteria);
    Task<PersonnelEffectivenessReportDto> GeneratePersonnelEffectivenessReportAsync(PersonnelReportCriteria criteria);
    Task<EquipmentReplacementReportDto> GenerateEquipmentReplacementReportAsync();
    Task<DepartmentTransferReportDto> GenerateDepartmentTransferReportAsync(string departmentId);
    Task<CorrelationAnalysisReportDto> GenerateCorrelationAnalysisReportAsync();
    Task<BonusDeterminationReportDto> GenerateBonusDeterminationReportAsync(BonusReportCriteria criteria);
}
