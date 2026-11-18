using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Assignment.Entities;

namespace Assignment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<HeroClass> HeroClasses { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<HeroQuest> HeroQuests { get; set; }
    }
}
