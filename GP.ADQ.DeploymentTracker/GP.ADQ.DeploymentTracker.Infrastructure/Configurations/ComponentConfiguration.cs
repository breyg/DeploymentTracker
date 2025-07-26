using GP.ADQ.DeploymentTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GP.ADQ.DeploymentTracker.Infrastructure.Configurations
{
    public class ComponentConfiguration : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> builder)
        {
            builder.ToTable("Components");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(e => e.JiraTicket)
                .HasMaxLength(50);

            builder.Property(e => e.MemoryAllocation)
                .HasMaxLength(20);

            builder.Property(e => e.Timeout)
                .HasMaxLength(20);

            // Relationships
            builder.HasMany<ComponentVersion>("Versions")
                .WithOne(v => v.Component)
                .HasForeignKey(v => v.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<ChecklistItem>("ChecklistItems")
                .WithOne(ci => ci.Component)
                .HasForeignKey(ci => ci.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(e => new { e.ProjectId, e.Name }).IsUnique();
        }
    }
}
