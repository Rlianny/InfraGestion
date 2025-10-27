using Domain.Aggregations;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PerformanceRatingRepository : Repository<PerformanceRating>, IPerformanceRatingRepository
    {
        public PerformanceRatingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PerformanceRating>> GetAssessmentsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(a => a.TechnicianID == technicianId)
                .Include(a => a.UserID)
                .Include(a => a.TechnicianID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PerformanceRating>> GetAssessmentsByUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(a => a.UserID == userId)
                .Include(a => a.TechnicianID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PerformanceRating>> GetAssessmentsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(a => a.Date >= startDate && a.Date <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<double> GetAverageScoreByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            var assessments = await _dbSet
                .Where(a => a.TechnicianID == technicianId)
                .ToListAsync(cancellationToken);

            if (!assessments.Any())
                return 0;

            return assessments.Average(a => a.Score);
        }

        public async Task<IEnumerable<PerformanceRating>> GetTopAssessmentsAsync(int count, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .OrderByDescending(a => a.Score)
                .Take(count)
                .Include(a => a.TechnicianID)
                .Include(a => a.UserID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PerformanceRating>> GetLowAssessmentsAsync(double thresholdScore, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(a => a.Score < thresholdScore)
                .Include(a => a.TechnicianID)
                .Include(a => a.UserID)
                .ToListAsync(cancellationToken);
        }
    }
}
