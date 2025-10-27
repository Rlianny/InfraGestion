using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetUsersByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
        Task<bool> UserExistsAsync(string userName, CancellationToken cancellationToken = default);
    }
}
