using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDirectorRepository : IRepository<Director>
    {
        Task<Director?> GetDirectorWithDetailsAsync(int directorId, CancellationToken cancellationToken = default);
    }
}
