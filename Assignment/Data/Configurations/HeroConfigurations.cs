using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Assignment.Entities;

namespace Assignment.Data.Configurations
{
    public class HeroConfigurations : IEntityTypeConfiguration<Hero>
    {
        public void Configure(EntityTypeBuilder<Hero> builder)
        {
            builder.Property(h => h.Name).IsRequired().HasMaxLength(50);
            builder.Property(h => h.OwnerId).IsRequired().HasMaxLength(450);
            builder.HasIndex(h => h.OwnerId);

            // One-to-Many: HeroClass -> Hero
            builder.HasOne(h => h.HeroClass)
                   .WithMany(c => c.Heroes)
                   .HasForeignKey(h => h.HeroClassId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Many-to-Many: Hero <-> Quest through HeroQuest
            builder.HasMany(h => h.Quests)
                   .WithMany(q => q.Heroes)
                   .UsingEntity<HeroQuest>(
                       r => r.HasOne(hq => hq.Quest).WithMany(q => q.HeroQuests).HasForeignKey(hq => hq.QuestId),
                       l => l.HasOne(hq => hq.Hero).WithMany(h => h.HeroQuests).HasForeignKey(hq => hq.HeroId),
                       j => j.HasKey(hq => new { hq.HeroId, hq.QuestId }));

            builder.HasQueryFilter(h => !h.IsDeleted);
        }
    }
}
