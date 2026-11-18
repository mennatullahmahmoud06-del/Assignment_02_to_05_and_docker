using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Assignment.Entities;

namespace Assignment.Data
{
    public static class StoreContextSeeding
    {
        private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!await dbContext.HeroClasses.AnyAsync())
            {
                var classes = await ReadAsync<HeroClass>("HeroClasses.json");
                if (classes.Count > 0)
                {
                    await dbContext.HeroClasses.AddRangeAsync(classes);
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!await dbContext.Quests.AnyAsync())
            {
                var quests = await ReadAsync<Quest>("Quests.json");
                if (quests.Count > 0)
                {
                    await dbContext.Quests.AddRangeAsync(quests);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private static async Task<List<T>> ReadAsync<T>(string fileName)
        {
            // Relative to the dll, so it works locally and inside Docker
            var path = Path.Combine(AppContext.BaseDirectory, "Data", "Seeding", fileName);
            if (!File.Exists(path))
                return [];

            await using var stream = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<List<T>>(stream, Options) ?? [];
        }
    }
}
