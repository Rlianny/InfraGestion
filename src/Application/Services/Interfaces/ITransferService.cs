using Application.DTOs.Transfer;

namespace Application.Services.Interfaces
{
    public interface ITransferService
    {
        Task InitiateTransferAsync(CreateTransferRequestDto request);
        Task ConfirmReceptionAsync(int deviceId, int userId);
        Task<IEnumerable<TransferDto>> GetPendingTransfersAsync();
        Task<TransferDto> GetTransferByIdAsync(int transferId);
        Task<IEnumerable<TransferDto>> GetTransfersByDeviceNameAsync(string deviceName);
        Task UpdateEquipmentLocationAsync(string deviceName, string newDepartmentName);
        Task DeleteTransferAsync(int transferId);
        Task DesactivateTransferAsync(int transferId);
        Task UpdateTransferAsync(UpdateTransferDto updateTransferDto);
        Task<IEnumerable<TransferDto>> GetPendingTransfersByLogisticianAsync(int LogisticId);
    }
}