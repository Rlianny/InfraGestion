using Domain.Aggregations;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DecommissioningRequestRepository
        : Repository<DecommissioningRequest>,
            IDecommissioningRequestRepository
    {
        public DecommissioningRequestRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<
            IEnumerable<DecommissioningRequest>
        > GetDecommissioningRequestsByTechnicianAsync(
            int technicianId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(dr => dr.TechnicianID == technicianId)
                .Include(dr => dr.DeviceID)
                .ToListAsync(cancellationToken);
        }

        public async Task<
            IEnumerable<DecommissioningRequest>
        > GetDecommissioningRequestsByDeviceAsync(
            int deviceId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(dr => dr.DeviceID == deviceId)
                .Include(dr => dr.TechnicianID)
                .ToListAsync(cancellationToken);
        }

        public async Task<
            IEnumerable<DecommissioningRequest>
        > GetPendingDecommissioningRequestsAsync(CancellationToken cancellationToken = default)
        {
            // Pending requests are those that don't have a corresponding Decommissioning record yet
            // This would require a more complex query joining with Decommissioning table
            return await _dbSet
                .Include(dr => dr.DeviceID)
                .Include(dr => dr.TechnicianID)
                .ToListAsync(cancellationToken);
        }

        public async Task<
            IEnumerable<DecommissioningRequest>
        > GetDecommissioningRequestsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(dr => dr.Date >= startDate && dr.Date <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountPendingDecommissioningRequestsAsync(
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.CountAsync(cancellationToken);
        }
    }
}
