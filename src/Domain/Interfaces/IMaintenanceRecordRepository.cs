using Domain.Aggregations;

namespace Domain.Interfaces
{
    public interface IMaintenanceRecordRepository : IRepository<MaintenanceRecord>
    {
        Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByDeviceAsync(int deviceId, CancellationToken cancellationToken = default);
        Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByTypeAsync(string type, CancellationToken cancellationToken = default);
        Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<double> GetTotalMaintenanceCostByDeviceAsync(int deviceId, CancellationToken cancellationToken = default);
        Task<double> GetTotalMaintenanceCostByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<int> CountMaintenancesByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
    }
}
