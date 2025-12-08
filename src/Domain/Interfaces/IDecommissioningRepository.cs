using Domain.Aggregations;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IDecommissioningRepository : IRepository<Decommissioning>
    {
        // A device can only have ONE finalized decommissioning
        Task<Decommissioning?> GetDecommissioningByDeviceAsync(int deviceId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Decommissioning>> GetDecommissioningsByReasonAsync(DecommissioningReason reason, CancellationToken cancellationToken = default);
        Task<IEnumerable<Decommissioning>> GetDecommissioningsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<IEnumerable<Decommissioning>> GetDecommissioningsByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
    }
}
