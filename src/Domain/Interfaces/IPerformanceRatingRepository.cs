using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPerformanceRatingRepository : IRepository<PerformanceRating>
    {
        Task<IEnumerable<PerformanceRating>> GetRatingsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PerformanceRating>> GetRatingsByUserAsync(int userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PerformanceRating>> GetRatingsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<double> GetAverageScoreByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
    }
}
