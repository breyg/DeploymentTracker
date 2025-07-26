using GP.ADQ.DeploymentTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GP.ADQ.DeploymentTracker.Infrastructure.Configurations
{
    public class ChecklistItemConfiguration : IEntityTypeConfiguration<ChecklistItem>
    {
        public void Configure(EntityTypeBuilder<ChecklistItem> builder)
        {
            builder.ToTable("ChecklistItems");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Notes)
                .HasMaxLength(1000);

            builder.Property(e => e.Order)
                .IsRequired();

            // Index for ordering
            builder.HasIndex(e => new { e.ComponentId, e.Order });
        }
    }
}
