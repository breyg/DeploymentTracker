using GP.ADQ.DeploymentTracker.Domain.Enums;

namespace GP.ADQ.DeploymentTracker.Domain.Entities
{
    public class Component : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ComponentType Type { get; set; }
        public int ProjectId { get; set; }
        public string? JiraTicket { get; set; }
        public string? MemoryAllocation { get; set; }
        public string? Timeout { get; set; }
        public virtual Project Project { get; set; } = null!;
        public virtual ICollection<ComponentVersion> Versions { get; set; } = new List<ComponentVersion>();
        public virtual ICollection<ChecklistItem> ChecklistItems { get; set; } = new List<ChecklistItem>();

        public Component() { }

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

        public void ClearChecklist()
        {
            ChecklistItems.Clear();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateInfo(string name, ComponentType type, string? jiraTicket)
        {
            Name = name;
            Type = type;
            JiraTicket = jiraTicket;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
