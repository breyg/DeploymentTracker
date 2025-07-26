using GP.ADQ.DeploymentTracker.Domain.Enums;

namespace GP.ADQ.DeploymentTracker.Domain.Entities
{
    public class ComponentVersion : BaseEntity
    {
        public int ComponentId { get; private set; }
        public EnvironmentType Environment { get; private set; }
        public string Version { get; private set; } = string.Empty;
        public DeploymentStatus Status { get; private set; }
        public DateTime? LastDeployDate { get; private set; }
        public string? DeployedBy { get; private set; }

        public Component Component { get; private set; } = null!;

        private ComponentVersion() { }

        public ComponentVersion(int componentId, EnvironmentType environment, string version, DeploymentStatus status)
        {
            ComponentId = componentId;
            Environment = environment;
            Version = version ?? throw new ArgumentNullException(nameof(version));
            Status = status;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateVersion(string version, DeploymentStatus status)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
            Status = status;

            if (status == DeploymentStatus.Deployed)
            {
                LastDeployDate = DateTime.UtcNow;
                // TODO: Get from authentication context
                DeployedBy = "system";
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public void Deploy(string deployedBy)
        {
            Status = DeploymentStatus.Deployed;
            LastDeployDate = DateTime.UtcNow;
            DeployedBy = deployedBy;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsFailed()
        {
            Status = DeploymentStatus.Failed;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
