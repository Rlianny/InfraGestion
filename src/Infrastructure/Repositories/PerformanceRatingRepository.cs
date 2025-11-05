using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PerformanceRatingRepository
        : Repository<PerformanceRating>,
            IPerformanceRatingRepository
    {
        public PerformanceRatingRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<IEnumerable<PerformanceRating>> GetRatingsByTechnicianAsync(
            int technicianId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(a => a.TechnicianID == technicianId)
                .Include(a => a.UserID)
                .Include(a => a.TechnicianID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PerformanceRating>> GetRatingsByUserAsync(
            int userId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(a => a.UserID == userId)
                .Include(a => a.TechnicianID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PerformanceRating>> GetRatingsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(a => a.Date >= startDate && a.Date <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<double> GetAverageScoreByTechnicianAsync(
            int technicianId,
            CancellationToken cancellationToken = default
        )
        {
            var Ratings = await _dbSet
                .Where(a => a.TechnicianID == technicianId)
                .ToListAsync(cancellationToken);

            if (!Ratings.Any())
                return 0;

            return Ratings.Average(a => a.Score);
        }
    }
}
