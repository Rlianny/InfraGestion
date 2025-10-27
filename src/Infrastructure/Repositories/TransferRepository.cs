using Domain.Aggregations;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TransferRepository : Repository<Transfer>, ITransferRepository
    {
        public TransferRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Transfer?> GetTransferWithDetailsAsync(int transferId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(t => t.DeviceID)
                .Include(t => t.SourceSectionID)
                .Include(t => t.DestinationSectionID)
                .Include(t => t.DeviceReceiverID)
                .FirstOrDefaultAsync(t => t.TransferID == transferId, cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByDeviceAsync(int deviceId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.DeviceID == deviceId)
                .Include(t => t.SourceSectionID)
                .Include(t => t.DestinationSectionID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetTransfersBySourceSectionAsync(int sourceSectionId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.SourceSectionID == sourceSectionId)
                .Include(t => t.DeviceID)
                .Include(t => t.DestinationSectionID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByDestinySectionAsync(int destinationSectionId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.DestinationSectionID == destinationSectionId)
                .Include(t => t.DeviceID)
                .Include(t => t.SourceSectionID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var startDateOnly = DateOnly.FromDateTime(startDate);
            var endDateOnly = DateOnly.FromDateTime(endDate);

            return await _dbSet
                .Where(t => t.Date >= startDateOnly && t.Date <= endDateOnly)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountTransfersBetweenSectionsAsync(int sourceSectionId, int destinySectionId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .CountAsync(t => t.SourceSectionID == sourceSectionId && t.DestinationSectionID == destinySectionId, cancellationToken);
        }
    }
}
