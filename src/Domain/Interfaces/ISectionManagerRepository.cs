using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISectionManagerRepository : IRepository<SectionManager>
    {
        Task<IEnumerable<SectionManager>> GetSectionManagersBySectionAsync(int sectionId, CancellationToken cancellationToken = default);
        Task<SectionManager?> GetSectionManagerForSectionAsync(int sectionId, CancellationToken cancellationToken = default);
    }
}
