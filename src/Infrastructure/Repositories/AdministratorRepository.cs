using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AdministratorRepository : Repository<Administrator>, IAdministratorRepository
    {
        public AdministratorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Administrator?> GetAdministratorWithDetailsAsync(int administratorId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.UserID == administratorId, cancellationToken);
        }
    }
}
