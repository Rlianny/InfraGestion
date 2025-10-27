using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SectionManagerRepository : Repository<SectionManager>, ISectionManagerRepository
    {
        public SectionManagerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<SectionManager?> GetSectionManagerWithDetailsAsync(int sectionManagerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(sm => sm.DepartmentID)
                .Include(sm => sm.SectionID)
                .FirstOrDefaultAsync(sm => sm.UserID == sectionManagerId, cancellationToken);
        }

        public async Task<IEnumerable<SectionManager>> GetSectionManagersBySectionAsync(int sectionId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(sm => sm.SectionID == sectionId)
                .ToListAsync(cancellationToken);
        }

        public async Task<SectionManager?> GetSectionManagerForSectionAsync(int sectionId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(sm => sm.SectionID == sectionId, cancellationToken);
        }
    }
}
