namespace GP.ADQ.DeploymentTracker.Application.DTOs
{
    public class CreateComponentDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? JiraTicket { get; set; }
        public int? MemoryAllocation { get; set; }
        public int? Timeout { get; set; }
    }

    public class UpdateComponentDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? JiraTicket { get; set; }
        public int? MemoryAllocation { get; set; }
        public int? Timeout { get; set; }
    }

    public class UpdateChecklistDto
    {
        public List<ChecklistItemRequest> Items { get; set; } = new();
    }

    public class ChecklistItemRequest
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string? Notes { get; set; }
        public int Order { get; set; }
    }

    public class ToggleChecklistItemDto
    {
        public bool Completed { get; set; }
    }
}