using Domain.Aggregations;

namespace Domain.Interfaces
{
    public interface IReceivingInspectionRequestRepository : IRepository<ReceivingInspectionRequest>
    {
        Task<IEnumerable<ReceivingInspectionRequest>> GetReceivingInspectionRequestsByDeviceAsync(int deviceId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReceivingInspectionRequest>> GetReceivingInspectionRequestsByAdministratorAsync(int administratorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReceivingInspectionRequest>> GetReceivingInspectionRequestsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReceivingInspectionRequest>> GetPendingInspectionRequestsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<ReceivingInspectionRequest>> GetAcceptedInspectionRequestsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<ReceivingInspectionRequest>> GetRejectedInspectionRequestsAsync(CancellationToken cancellationToken = default);
    }
}
