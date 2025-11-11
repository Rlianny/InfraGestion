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
                .Include(d => d.SectionId)
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
            if (departmentName == null)
            {
                throw new Exception("DepartmentName can not be null");
            }
            return await _dbSet
            .FirstOrDefaultAsync(d => d.Name != null && d.Name == departmentName, cancellationToken);
        }
    }
}
