using Microsoft.EntityFrameworkCore;
using Assignment.Data;
using Assignment.Entities;

namespace Assignment.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext DbContext;

        public GenericRepository(ApplicationDbContext dbContext)
            => DbContext = dbContext;

        public async Task<IEnumerable<T>> GetAllAsync()
            => await DbContext.Set<T>().AsNoTracking().ToListAsync();

        // FirstOrDefaultAsync (not FindAsync) so the soft-delete query filter is applied
        public async Task<T?> GetByIdAsync(int id)
            => await DbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> ExistsAsync(int id)
            => await DbContext.Set<T>().AnyAsync(x => x.Id == id);

        public async Task<T> CreateAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            // Entities loaded by this context are already tracked; only attach detached ones
            if (DbContext.Entry(entity).State == EntityState.Detached)
                DbContext.Set<T>().Update(entity);

            await DbContext.SaveChangesAsync();
            return true;
        }

        // The SoftDeleteInterceptor turns this Remove into IsDeleted = true
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null)
                return false;

            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
            return true;
        }
    }
}
