using Microsoft.EntityFrameworkCore;
using Assignment.Data;
using Assignment.Entities;

namespace Assignment.Repositories
{
    public class HeroClassRepository : GenericRepository<HeroClass>, IHeroClassRepository
    {
        public HeroClassRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<HeroClass>> GetAllWithHeroesAsync()
            => await DbContext.HeroClasses
                              .Include(c => c.Heroes)
                              .AsNoTracking()
                              .OrderBy(c => c.Name)
                              .ToListAsync();

        public async Task<HeroClass?> GetByIdWithHeroesAsync(int id, bool asNoTracking = true)
        {
            IQueryable<HeroClass> query = DbContext.HeroClasses.Include(c => c.Heroes);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
            => await DbContext.HeroClasses.AnyAsync(c => c.Name == name && (excludeId == null || c.Id != excludeId));
    }
}
