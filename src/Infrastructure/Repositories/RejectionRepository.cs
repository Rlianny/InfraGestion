using Domain.Aggregations;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RejectionRepository : Repository<Rejection>, IRejectionRepository
    {
        public RejectionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Rejection>> GetRejectionsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.TechnicianID == technicianId)
                .Include(r => r.DeviceID)
                .Include(r => r.DeviceReceiverID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Rejection>> GetRejectionsByDeviceAsync(int deviceId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.DeviceID == deviceId)
                .Include(r => r.TechnicianID)
                .Include(r => r.DeviceReceiverID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Rejection>> GetRejectionsByDeviceReceiverAsync(int DeviceReceiverId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.DeviceReceiverID == DeviceReceiverId)
                .Include(r => r.DeviceID)
                .Include(r => r.TechnicianID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Rejection>> GetRejectionsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.RejectionDate >= startDate && r.RejectionDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountRejectionsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .CountAsync(r => r.TechnicianID == technicianId, cancellationToken);
        }
    }
}
