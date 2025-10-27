using Domain.Aggregations;

namespace Domain.Interfaces
{
    public interface IMaintenanceRepository : IRepository<Mainteinance>
    {
        Task<IEnumerable<Mainteinance>> GetMaintenancesByDeviceAsync(int deviceId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Mainteinance>> GetMaintenancesByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Mainteinance>> GetMaintenancesByTypeAsync(string type, CancellationToken cancellationToken = default);
        Task<IEnumerable<Mainteinance>> GetMaintenancesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<double> GetTotalMaintenanceCostByDeviceAsync(int deviceId, CancellationToken cancellationToken = default);
        Task<double> GetTotalMaintenanceCostByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<int> CountMaintenancesByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
    }
}
