using Application.DTOs.Transfer;

namespace Application.Services.Interfaces
{
    public interface ITransferService
    {
        Task InitiateTransferAsync(CreateTransferRequestDto request);
        Task ConfirmReceptionAsync(int transferId);
        Task<IEnumerable<TransferDto>> GetPendingTransfersAsync();
        Task<TransferDto> GetTransferByIdAsync(int transferId);
        Task<List<TransferDto>> GetTransfersByDeviceAsync(int deviceId);
        Task UpdateEquipmentLocationAsync(int equipmentId, int newDepartmentId);
    }
}