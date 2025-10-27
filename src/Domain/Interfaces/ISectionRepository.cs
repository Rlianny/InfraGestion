using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISectionRepository : IRepository<Section>
    {
        Task<Section?> GetSectionByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
