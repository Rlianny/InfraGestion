using Domain.Aggregations;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TransferRepository : Repository<Transfer>, ITransferRepository
    {
        public TransferRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<Transfer?> GetTransferWithDetailsAsync(
            int transferId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.TransferId == transferId, cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByDeviceAsync(
            int deviceId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(t => t.DeviceId == deviceId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetTransfersBySourceSectionAsync(
            int sourceSectionId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(t => t.SourceSectionId == sourceSectionId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByDestinySectionAsync(
            int destinationSectionId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(t => t.DestinationSectionId == destinationSectionId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        )
        {
            var startDateOnly = DateOnly.FromDateTime(startDate);
            var endDateOnly = DateOnly.FromDateTime(endDate);

            return await _dbSet
                .Where(t => t.Date >= startDateOnly && t.Date <= endDateOnly)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountTransfersBetweenSectionsAsync(
            int sourceSectionId,
            int destinySectionId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.CountAsync(
                t =>
                    t.SourceSectionId == sourceSectionId
                    && t.DestinationSectionId == destinySectionId,
                cancellationToken
            );
        }
    }
}
