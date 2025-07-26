namespace GP.ADQ.DeploymentTracker.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string CreatedBy { get; private set; } = string.Empty;
        public virtual ICollection<Component> Components { get; private set; } = new List<Component>();

        private Project() { }

        public Project(string name, string description, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be empty", nameof(name));

            Name = name;
            Description = description ?? string.Empty;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateInfo(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be empty", nameof(name));

            Name = name;
            Description = description ?? string.Empty;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddComponent(Component component)
        {
            if (Components.Any(c => c.Name.Equals(component.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Component '{component.Name}' already exists in this project");

            Components.Add(component);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
