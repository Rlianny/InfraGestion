using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DirectorRepository : Repository<Director>, IDirectorRepository
    {
        public DirectorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Director?> GetDirectorWithDetailsAsync(int directorId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.UserID == directorId, cancellationToken);
        }
    }
}
