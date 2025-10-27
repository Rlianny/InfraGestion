using Domain.Aggregations;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AssessmentRepository : Repository<Assessments>, IAssessmentRepository
    {
        public AssessmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Assessments>> GetAssessmentsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(a => a.TechnicianID == technicianId)
                .Include(a => a.User)
                .Include(a => a.Technician)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Assessments>> GetAssessmentsByUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(a => a.UserID == userId)
                .Include(a => a.Technician)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Assessments>> GetAssessmentsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
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

        public async Task<IEnumerable<Assessments>> GetTopAssessmentsAsync(int count, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .OrderByDescending(a => a.Score)
                .Take(count)
                .Include(a => a.Technician)
                .Include(a => a.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Assessments>> GetLowAssessmentsAsync(double thresholdScore, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(a => a.Score < thresholdScore)
                .Include(a => a.Technician)
                .Include(a => a.User)
                .ToListAsync(cancellationToken);
        }
    }
}
