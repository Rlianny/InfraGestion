using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITechnicianRepository : IRepository<Technician>
    {
        Task<Technician?> GetTechnicianWithDetailsAsync(int technicianId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Technician>> GetTechniciansBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default);
        Task<IEnumerable<Technician>> GetTechniciansByExperienceAsync(int minYears, CancellationToken cancellationToken = default);
        Task<IEnumerable<Technician>> GetTechniciansByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Technician>> GetTopPerformingTechniciansAsync(int count, CancellationToken cancellationToken = default);
    }
}
