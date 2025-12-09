using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISectionRepository : IRepository<Section>
    {
        Task<Section?> GetSectionByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Section>> SearchSectionsByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<bool> SectionExistsAsync(string name, CancellationToken cancellationToken = default);
    }
}
