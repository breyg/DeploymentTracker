using GP.ADQ.DeploymentTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GP.ADQ.DeploymentTracker.Infrastructure.Data
{
    public class DeploymentTrackerContext : DbContext
    {
        public DeploymentTrackerContext(DbContextOptions<DeploymentTrackerContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Component> Components { get; set; } = null!;
        public DbSet<ComponentVersion> ComponentVersions { get; set; } = null!;
        public DbSet<ChecklistItem> ChecklistItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeploymentTrackerContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
