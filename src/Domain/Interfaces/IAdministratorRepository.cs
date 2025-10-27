using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAdministratorRepository : IRepository<Administrator>
    {
        Task<Administrator?> GetAdministratorWithDetailsAsync(int administratorId, CancellationToken cancellationToken = default);
    }
}
