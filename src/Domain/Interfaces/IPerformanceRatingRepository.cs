using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPerformanceRatingRepository : IRepository<PerformanceRating>
    {
        Task<IEnumerable<PerformanceRating>> GetAssessmentsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PerformanceRating>> GetAssessmentsByUserAsync(int userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PerformanceRating>> GetAssessmentsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<double> GetAverageScoreByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
    }
}
