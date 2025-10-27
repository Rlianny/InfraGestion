using Domain.Aggregations;

namespace Domain.Interfaces
{
    public interface IRejectionRepository : IRepository<Rejection>
    {
        Task<IEnumerable<Rejection>> GetRejectionsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Rejection>> GetRejectionsByDeviceAsync(int deviceId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Rejection>> GetRejectionsByDeviceReceiverAsync(int DeviceReceiverId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Rejection>> GetRejectionsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
