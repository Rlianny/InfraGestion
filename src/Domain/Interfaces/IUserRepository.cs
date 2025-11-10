using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<User?> GetUserWithDetailsAsync(int userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetUsersByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
        Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);
        public Task<IEnumerable<User>> GetAllUsersWithDetailsAsync(
            CancellationToken cancellationToken = default
        );
    }
}
