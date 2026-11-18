using Microsoft.EntityFrameworkCore;
using Assignment.Data;
using Assignment.Entities;
using Assignment.Results;

namespace Assignment.Repositories
{
    public class HeroRepository : GenericRepository<Hero>, IHeroRepository
    {
        public HeroRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PaginatedList<Hero>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var paginatedList = new PaginatedList<Hero>
            {
                PageNumber = Math.Max(pageNumber, 1),
                PageSize = Math.Max(pageSize, 1)   // the setter caps it at 20
            };

            IQueryable<Hero> query = DbContext.Heroes.Include(h => h.HeroClass).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(h => h.Name.Contains(searchTerm));

            paginatedList.TotalRecords = await query.CountAsync();
            paginatedList.TotalPages = (int)Math.Ceiling(paginatedList.TotalRecords / (double)paginatedList.PageSize);
            paginatedList.Data = await query
                .OrderBy(h => h.Id)
                .Skip((paginatedList.PageNumber - 1) * paginatedList.PageSize)
                .Take(paginatedList.PageSize)
                .ToListAsync();

            return paginatedList;
        }

        public async Task<IEnumerable<Hero>> GetByOwnerAsync(string ownerId)
            => await DbContext.Heroes
                              .Include(h => h.HeroClass)
                              .AsNoTracking()
                              .Where(h => h.OwnerId == ownerId)
                              .OrderBy(h => h.Id)
                              .ToListAsync();

        public async Task<IEnumerable<Hero>> GetTopAsync(int count)
            => await DbContext.Heroes
                              .Include(h => h.HeroClass)
                              .AsNoTracking()
                              .OrderByDescending(h => h.Level)
                              .ThenByDescending(h => h.Xp)
                              .Take(count)
                              .ToListAsync();

        public async Task<int> CountByOwnerAsync(string ownerId)
            => await DbContext.Heroes.CountAsync(h => h.OwnerId == ownerId);

        // Navigation properties: Hero -> HeroClass and Hero -> HeroQuests -> Quest
        public async Task<Hero?> GetWithDetailsAsync(int id, bool asNoTracking = true)
        {
            IQueryable<Hero> query = DbContext.Heroes
                                              .Include(h => h.HeroClass)
                                              .Include(h => h.HeroQuests)
                                                  .ThenInclude(hq => hq.Quest);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(h => h.Id == id);
        }
    }
}
