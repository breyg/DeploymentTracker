using GP.ADQ.DeploymentTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GP.ADQ.DeploymentTracker.Infrastructure.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Description)
                .HasMaxLength(1000);

            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt);

            // Index for performance
            builder.HasIndex(e => e.Name).IsUnique();
            builder.HasIndex(e => e.CreatedAt);

            // Relationships
            builder.HasMany<Component>("Components")
                .WithOne(c => c.Project)
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
