namespace GP.ADQ.DeploymentTracker.Domain.Entities
{
    public class ChecklistItem : BaseEntity
    {
        public int ComponentId { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public bool IsCompleted { get; private set; }
        public string? Notes { get; private set; }
        public int Order { get; private set; }

        public Component Component { get; private set; } = null!;

        private ChecklistItem() { }

        public ChecklistItem(int componentId, string description, int order)
        {
            ComponentId = componentId;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Order = order;
            CreatedAt = DateTime.UtcNow;
        }

        public void Complete(string? notes = null)
        {
            IsCompleted = true;
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Uncomplete()
        {
            IsCompleted = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDescription(string description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
