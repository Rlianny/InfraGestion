using Domain.Aggregations;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DecommissioningRepository : Repository<Decommissioning>, IDecommissioningRepository
    {
        public DecommissioningRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Decommissioning?> GetDecommissioningWithDetailsAsync(int decommissioningId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(d => d.Device)
                .Include(d => d.DeviceReceiver)
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.DecommissioningRequestID == decommissioningId, cancellationToken);
        }

        public async Task<IEnumerable<Decommissioning>> GetDecommissioningsByDeviceAsync(int deviceId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(d => d.DeviceID == deviceId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Decommissioning>> GetDecommissioningsByReasonAsync(DecommissioningReason reason, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(d => d.Reason == reason)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Decommissioning>> GetDecommissioningsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(d => d.DecommissioningDate >= startDate && d.DecommissioningDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Decommissioning>> GetDecommissioningsByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(d => d.ReceiverDepartmentID == departmentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountDecommissioningsByReasonAsync(DecommissioningReason reason, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .CountAsync(d => d.Reason == reason, cancellationToken);
        }
    }
}
