using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<User?> GetByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Include(u => u.Department)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }

        public async Task<User?> GetUserWithDetailsAsync(
            int userId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Include(u => u.Department)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
        }
        public async Task<IEnumerable<User>> GetAllUsersWithDetailsAsync(
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Include(u => u.Department)
                .Include(u => u.Role)
                .OrderBy(u => u.FullName)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetUsersByDepartmentAsync(
            int departmentId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(u => u.DepartmentId == departmentId)
                .Include(u => u.Department)
                .Include(u => u.Role)
                .OrderBy(u => u.FullName)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(
            int roleId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(u => u.RoleId == roleId)
                .Include(u => u.Department)
                .Include(u => u.Role)
                .OrderBy(u => u.FullName)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync(
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(u => u.IsActive)
                .Include(u => u.Department)
                .Include(u => u.Role)
                .OrderBy(u => u.FullName)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> UsernameExistsAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.AnyAsync(u => u.Username == username, cancellationToken);
        }
    }
}
