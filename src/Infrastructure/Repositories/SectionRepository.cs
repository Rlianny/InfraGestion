using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SectionRepository : Repository<Section>, ISectionRepository
    {
        public SectionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Section?> GetSectionByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
        }

        public async Task<IEnumerable<Section>> SearchSectionsByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(s => s.Name.Contains(searchTerm))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> SectionExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(s => s.Name == name, cancellationToken);
        }
    }
}
