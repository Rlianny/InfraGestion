using Domain.Aggregations;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DecommissioningRepository : Repository<Decommissioning>, IDecommissioningRepository
    {
        public DecommissioningRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<Decommissioning?> GetDecommissioningWithDetailsAsync(
            int decommissioningId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Include(d => d.DeviceId)
                .Include(d => d.DeviceReceiverId)
                .Include(d => d.ReceiverDepartmentId)
                .FirstOrDefaultAsync(
                    d => d.DecommissioningRequestId == decommissioningId,
                    cancellationToken
                );
        }

        // Un dispositivo solo puede tener UNA baja final
        public async Task<Decommissioning?> GetDecommissioningByDeviceAsync(
            int deviceId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.DeviceId == deviceId, cancellationToken);
        }

        public async Task<IEnumerable<Decommissioning>> GetDecommissioningsByReasonAsync(
            DecommissioningReason reason,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.Where(d => d.Reason == reason).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Decommissioning>> GetDecommissioningsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(d => d.DecommissioningDate >= startDate && d.DecommissioningDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Decommissioning>> GetDecommissioningsByDepartmentAsync(
            int departmentId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(d => d.ReceiverDepartmentId == departmentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountDecommissioningsByReasonAsync(
            DecommissioningReason reason,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.CountAsync(d => d.Reason == reason, cancellationToken);
        }
    }
}
