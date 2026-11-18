using Assignment.Entities;
using Assignment.Results;

namespace Assignment.Repositories
{
    public interface IHeroRepository : IGenericRepository<Hero>
    {
        Task<PaginatedList<Hero>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<IEnumerable<Hero>> GetByOwnerAsync(string ownerId);
        Task<IEnumerable<Hero>> GetTopAsync(int count);
        Task<int> CountByOwnerAsync(string ownerId);
        Task<Hero?> GetWithDetailsAsync(int id, bool asNoTracking = true);
    }
}
