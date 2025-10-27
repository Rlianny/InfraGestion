using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.FullName == userName, cancellationToken);
        }

        public async Task<User?> GetUserWithDepartmentAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(u => u.DepartmentID)
                .FirstOrDefaultAsync(u => u.UserID == userId, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetUsersByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(u => u.DepartmentID == departmentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> UserExistsAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(u => u.FullName == userName, cancellationToken);
        }
    }
}
