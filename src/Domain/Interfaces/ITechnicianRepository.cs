using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITechnicianRepository : IRepository<User>
    {
        Task<User?> GetTechnicianWithDetailsAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetTechniciansBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetTechniciansByExperienceAsync(int minYears, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetTechniciansByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetTopPerformingTechniciansAsync(int count, CancellationToken cancellationToken = default);
    }
}
