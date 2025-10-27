using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<IEnumerable<Department>> GetDepartmentsBySectionAsync(int sectionId, CancellationToken cancellationToken = default);
        Task<int> CountDepartmentsInSectionAsync(int sectionId, CancellationToken cancellationToken = default);
    }
}
