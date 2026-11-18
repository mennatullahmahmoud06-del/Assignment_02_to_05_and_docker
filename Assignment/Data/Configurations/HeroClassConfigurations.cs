using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Assignment.Entities;

namespace Assignment.Data.Configurations
{
    public class HeroClassConfigurations : IEntityTypeConfiguration<HeroClass>
    {
        public void Configure(EntityTypeBuilder<HeroClass> builder)
        {
            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Description).HasMaxLength(200);

            // Unique among the non-deleted rows only (soft delete keeps old rows)
            builder.HasIndex(c => c.Name).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
