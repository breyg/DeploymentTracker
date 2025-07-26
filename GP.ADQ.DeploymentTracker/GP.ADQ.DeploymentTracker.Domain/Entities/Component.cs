using GP.ADQ.DeploymentTracker.Domain.Enums;

namespace GP.ADQ.DeploymentTracker.Domain.Entities
{
    public class Component : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public ComponentType Type { get; private set; }
        public int ProjectId { get; private set; }
        public string? JiraTicket { get; private set; }
        public string? MemoryAllocation { get; private set; }
        public string? Timeout { get; private set; }
        public virtual Project Project { get; private set; } = null!;
        public virtual ICollection<ComponentVersion> Versions { get; private set; } = new List<ComponentVersion>();
        public virtual ICollection<ChecklistItem> ChecklistItems { get; private set; } = new List<ChecklistItem>();

        private Component() { }

        public Component(string name, ComponentType type, int projectId, string? jiraTicket = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Component name cannot be empty", nameof(name));

            Name = name;
            Type = type;
            ProjectId = projectId;
            JiraTicket = jiraTicket;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateConfiguration(string? memoryAllocation, string? timeout, string? jiraTicket)
        {
            MemoryAllocation = memoryAllocation;
            Timeout = timeout;
            JiraTicket = jiraTicket;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetVersion(EnvironmentType environment, string version, DeploymentStatus status)
        {
            var existingVersion = Versions.FirstOrDefault(v => v.Environment == environment);

            if (existingVersion != null)
            {
                existingVersion.UpdateVersion(version, status);
            }
            else
            {
                var newVersion = new ComponentVersion(Id, environment, version, status);
                Versions.Add(newVersion);
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public void AddChecklistItem(string description, int order)
        {
            var item = new ChecklistItem(Id, description, order);
            ChecklistItems.Add(item);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
