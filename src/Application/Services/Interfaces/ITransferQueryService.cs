public interface ITransferQueryService
{
    Task<List<TransferRequestDto>> GetTransfersBySectionAsync(string sectionId);
    Task<List<TransferRequestDto>> GetTransfersByDateRangeAsync(DateRange dateRange);
    Task<bool> CanEquipmentBeTransferredAsync(string equipmentId);
}