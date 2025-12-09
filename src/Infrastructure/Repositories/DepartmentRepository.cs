using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<Department?> GetDepartmentWithSectionAsync(
            int departmentId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Include(d => d.Section)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId, cancellationToken);
        }

        public async Task<IEnumerable<Department>> GetDepartmentsBySectionAsync(
            int sectionId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.Where(d => d.SectionId == sectionId).ToListAsync(cancellationToken);
        }

        public async Task<int> CountDepartmentsInSectionAsync(
            int sectionId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.CountAsync(d => d.SectionId == sectionId, cancellationToken);
        }

        public async Task<Department?> GetDepartmentByNameAsync(string departmentName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
            {
                throw new ArgumentNullException(nameof(departmentName), "Department name cannot be null or empty");
            }
            return await _dbSet
                .FirstOrDefaultAsync(d => d.Name == departmentName, cancellationToken);
        }
    }
}
