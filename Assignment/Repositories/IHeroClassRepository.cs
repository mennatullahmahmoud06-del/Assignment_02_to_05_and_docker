using Assignment.Entities;

namespace Assignment.Repositories
{
    public interface IHeroClassRepository : IGenericRepository<HeroClass>
    {
        Task<IEnumerable<HeroClass>> GetAllWithHeroesAsync();
        Task<HeroClass?> GetByIdWithHeroesAsync(int id, bool asNoTracking = true);
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
    }
}
