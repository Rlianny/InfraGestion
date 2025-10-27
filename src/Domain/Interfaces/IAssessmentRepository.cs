using Domain.Aggregations;

namespace Domain.Interfaces
{
    public interface IAssessmentRepository : IRepository<Assessments>
    {
        Task<IEnumerable<Assessments>> GetAssessmentsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Assessments>> GetAssessmentsByUserAsync(int userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Assessments>> GetAssessmentsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<double> GetAverageScoreByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default);
    }
}
