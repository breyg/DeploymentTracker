using GP.ADQ.DeploymentTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GP.ADQ.DeploymentTracker.Infrastructure.Configurations
{
    public class ComponentVersionConfiguration : IEntityTypeConfiguration<ComponentVersion>
    {
        public void Configure(EntityTypeBuilder<ComponentVersion> builder)
        {
            builder.ToTable("ComponentVersions");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Environment)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(e => e.Version)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(e => e.DeployedBy)
                .HasMaxLength(100);

            // Unique constraint: one version per component per environment
            builder.HasIndex(e => new { e.ComponentId, e.Environment })
                .IsUnique();
        }
    }
}
