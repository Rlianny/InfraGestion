public interface IExportService
{
    Task<byte[]> ExportToPdfAsync(object reportData);
    Task<byte[]> ExportInventoryToPdfAsync(InventoryReportCriteria criteria);
    Task<byte[]> ExportDischargesToPdfAsync(DischargeReportCriteria criteria);
}