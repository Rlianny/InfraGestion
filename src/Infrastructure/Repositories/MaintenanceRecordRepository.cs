using Domain.Aggregations;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MaintenanceRepository : Repository<MaintenanceRecord>, IMaintenanceRecordRepository
    {
        public MaintenanceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByDeviceAsync(int deviceId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(m => m.DeviceID == deviceId)
                .Include(m => m.Technician)
                .Include(m => m.Device)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(m => m.TechnicianID == technicianId)
                .Include(m => m.Device)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByTypeAsync(string type, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(m => m.Type == type)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var startDateOnly = DateOnly.FromDateTime(startDate);
            var endDateOnly = DateOnly.FromDateTime(endDate);

            return await _dbSet
                .Where(m => m.Date >= startDateOnly && m.Date <= endDateOnly)
                .ToListAsync(cancellationToken);
        }

        public async Task<double> GetTotalMaintenanceCostByDeviceAsync(int deviceId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(m => m.DeviceID == deviceId)
                .SumAsync(m => m.Cost, cancellationToken);
        }

        public async Task<double> GetTotalMaintenanceCostByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(m => m.TechnicianID == technicianId)
                .SumAsync(m => m.Cost, cancellationToken);
        }

        public async Task<int> CountMaintenancesByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .CountAsync(m => m.TechnicianID == technicianId, cancellationToken);
        }
    }
}
