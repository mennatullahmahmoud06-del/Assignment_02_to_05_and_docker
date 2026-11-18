using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Assignment.Entities;

namespace Assignment.Data.Interceptors
{
    // Turns every Remove() on a BaseEntity into IsDeleted = true (the query filters hide those rows)
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ApplySoftDelete(eventData);
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ApplySoftDelete(eventData);
            return ValueTask.FromResult(result);
        }

        private static void ApplySoftDelete(DbContextEventData eventData)
        {
            if (eventData.Context is null)
                return;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry.State != EntityState.Deleted || entry.Entity is not BaseEntity entity)
                    continue;

                entry.State = EntityState.Modified;
                entity.IsDeleted = true;
                entity.ModifiedAt = DateTime.UtcNow;
            }
        }
    }
}
