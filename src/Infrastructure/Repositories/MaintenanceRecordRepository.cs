using Domain.Aggregations;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MaintenanceRepository : Repository<MaintenanceRecord>, IMaintenanceRecordRepository
    {
        public MaintenanceRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByDeviceAsync(
            int deviceId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(m => m.DeviceId == deviceId)
                .Include(m => m.TechnicianId)
                .Include(m => m.DeviceId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByTechnicianAsync(
            int technicianId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(m => m.TechnicianId == technicianId)
                .Include(m => m.DeviceId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByTypeAsync(
            MaintenanceType type,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.Where(m => m.Type == type).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MaintenanceRecord>> GetMaintenancesByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(m => m.Date >= startDate && m.Date <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<double> GetTotalMaintenanceCostByDeviceAsync(
            int deviceId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(m => m.DeviceId == deviceId)
                .SumAsync(m => m.Cost, cancellationToken);
        }

        public async Task<double> GetTotalMaintenanceCostByTechnicianAsync(
            int technicianId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(m => m.TechnicianId == technicianId)
                .SumAsync(m => m.Cost, cancellationToken);
        }

        public async Task<int> CountMaintenancesByTechnicianAsync(
            int technicianId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.CountAsync(m => m.TechnicianId == technicianId, cancellationToken);
        }
    }
}
