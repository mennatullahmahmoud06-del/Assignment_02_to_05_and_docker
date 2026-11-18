using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Assignment.Entities;

namespace Assignment.Data.Configurations
{
    public class QuestConfigurations : IEntityTypeConfiguration<Quest>
    {
        public void Configure(EntityTypeBuilder<Quest> builder)
        {
            builder.Property(q => q.Title).IsRequired().HasMaxLength(100);
            builder.Property(q => q.Description).HasMaxLength(300);

            builder.HasQueryFilter(q => !q.IsDeleted);
        }
    }
}
