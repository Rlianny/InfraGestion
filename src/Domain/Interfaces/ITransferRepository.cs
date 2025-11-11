using Domain.Aggregations;

namespace Domain.Interfaces
{
    public interface ITransferRepository : IRepository<Transfer>
    {
        Task<Transfer?> GetTransferWithDetailsAsync(int transferId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transfer>> GetTransfersByDeviceAsync(int deviceId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transfer>> GetTransfersBySourceSectionAsync(int sourceSectionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transfer>> GetTransfersByDestinySectionAsync(int destinySectionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transfer>> GetTransfersByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
