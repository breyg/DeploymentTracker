using GP.ADQ.DeploymentTracker.Domain.Enums;

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

    public class UpdateVersionDto
    {
        public string Environment { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? DeployedBy { get; set; }
    }

    public class DeployComponentDto
    {
        public string Environment { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string DeployedBy { get; set; } = string.Empty;
    }
}