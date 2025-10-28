public interface ITransferService
{
    Task<TransferRequestDto> InitiateTransferAsync(InitiateTransferRequest request);
    Task<TransferRequestDto> ConfirmReceptionAsync(string transferId, ConfirmReceptionRequest request);
    Task<List<TransferRequestDto>> GetPendingTransfersAsync();
    Task<TransferRequestDto> GetTransferByIdAsync(string transferId);
    Task<List<TransferRequestDto>> GetTransfersByEquipmentAsync(string equipmentId);
    Task UpdateEquipmentLocationAsync(string equipmentId, string newLocation);
}
