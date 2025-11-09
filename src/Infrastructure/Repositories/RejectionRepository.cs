using Domain.Aggregations;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RejectionRepository : Repository<Rejection>, IRejectionRepository
    {
        public RejectionRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<IEnumerable<Rejection>> GetRejectionsByTechnicianAsync(
            int technicianId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(r => r.TechnicianId == technicianId)
                .Include(r => r.DeviceId)
                .Include(r => r.DeviceReceiverId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Rejection>> GetRejectionsByDeviceAsync(
            int deviceId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(r => r.DeviceId == deviceId)
                .Include(r => r.TechnicianId)
                .Include(r => r.DeviceReceiverId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Rejection>> GetRejectionsByDeviceReceiverAsync(
            int DeviceReceiverId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(r => r.DeviceReceiverId == DeviceReceiverId)
                .Include(r => r.DeviceId)
                .Include(r => r.TechnicianId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Rejection>> GetRejectionsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(r => r.RejectionDate >= startDate && r.RejectionDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountRejectionsByTechnicianAsync(
            int technicianId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.CountAsync(r => r.TechnicianId == technicianId, cancellationToken);
        }
    }
}
