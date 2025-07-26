using System.ComponentModel.DataAnnotations;

namespace GP.ADQ.DeploymentTracker.Application.DTOs
{
    public record ProjectDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public int ComponentsCount { get; init; }
        public List<ComponentDto> Components { get; init; } = new();
    }

    public record CreateProjectDto
    {
        [Required(ErrorMessage = "Project name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Project name must be between 3 and 200 characters")]
        public string Name { get; init; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; init; } = string.Empty;
    }

    public record UpdateProjectDto
    {
        [Required(ErrorMessage = "Project name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Project name must be between 3 and 200 characters")]
        public string Name { get; init; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; init; } = string.Empty;
    }

    public record ComponentDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string? JiraTicket { get; init; }
        public string? MemoryAllocation { get; init; }
        public string? Timeout { get; init; }
        public Dictionary<string, VersionDto> Versions { get; init; } = new();
        public List<ChecklistItemDto> Checklist { get; init; } = new();
    }

    public record VersionDto
    {
        public string Version { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public string? LastDeploy { get; init; }
    }

    public record ChecklistItemDto
    {
        public int Id { get; init; }
        public string Item { get; init; } = string.Empty;
        public bool Completed { get; init; }
        public string? Notes { get; init; }
    }
}
