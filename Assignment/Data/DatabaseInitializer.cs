using Microsoft.EntityFrameworkCore;

namespace Assignment.Data
{
    public static class DatabaseInitializer
    {
        // Uses migrations when the project has them (dotnet ef migrations add ...),
        // otherwise creates the schema directly so the project runs out of the box.
        public static async Task InitializeAsync(DbContext context)
        {
            if ((await context.Database.GetMigrationsAsync()).Any())
                await context.Database.MigrateAsync();
            else
                await context.Database.EnsureCreatedAsync();
        }
    }
}
