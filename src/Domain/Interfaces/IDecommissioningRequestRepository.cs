using Domain.Aggregations;

namespace Domain.Interfaces
{
     public interface IDecommissioningRequestRepository : IRepository<DecommissioningRequest>
    {
        Task<IEnumerable<DecommissioningRequest>> GetDecommissioningRequestsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<IEnumerable<DecommissioningRequest>> GetDecommissioningRequestsByDeviceAsync(int deviceId, CancellationToken cancellationToken = default);
        Task<IEnumerable<DecommissioningRequest>> GetPendingDecommissioningRequestsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<DecommissioningRequest>> GetDecommissioningRequestsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<int> CountPendingDecommissioningRequestsAsync(CancellationToken cancellationToken = default);
    }
}
