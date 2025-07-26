using GP.ADQ.DeploymentTracker.Application.DTOs;
using GP.ADQ.DeploymentTracker.Application.Interfaces;
using GP.ADQ.DeploymentTracker.Domain.Entities;

namespace GP.ADQ.DeploymentTracker.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<List<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return projects.Select(MapToDto).ToList();
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdWithComponentsAsync(id);
            return project == null ? null : MapToDto(project);
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto request)
        {
            // Validar nombre único
            if (await _projectRepository.NameExistsAsync(request.Name))
                throw new InvalidOperationException($"Project with name '{request.Name}' already exists");

            var project = new Project(request.Name, request.Description, "system"); // TODO: Get from auth
            var createdProject = await _projectRepository.AddAsync(project);

            return MapToDto(createdProject);
        }

        public async Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto request)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;

            // Validar nombre único (excluyendo el actual)
            if (await _projectRepository.NameExistsAsync(request.Name, id))
                throw new InvalidOperationException($"Project with name '{request.Name}' already exists");

            project.UpdateInfo(request.Name, request.Description);
            var updatedProject = await _projectRepository.UpdateAsync(project);

            return MapToDto(updatedProject);
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            if (!await _projectRepository.ExistsAsync(id))
                return false;

            await _projectRepository.DeleteAsync(id);
            return true;
        }

        private static ProjectDto MapToDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                ComponentsCount = project.Components.Count,
                Components = project.Components.Select(MapComponentToDto).ToList()
            };
        }

        private static ComponentDto MapComponentToDto(Component component)
        {
            return new ComponentDto
            {
                Id = component.Id,
                Name = component.Name,
                Type = component.Type.ToString(),
                JiraTicket = component.JiraTicket,
                MemoryAllocation = component.MemoryAllocation,
                Timeout = component.Timeout,
                Versions = component.Versions.ToDictionary(
                    v => v.Environment.ToString().ToLower(),
                    v => new VersionDto
                    {
                        Version = v.Version,
                        Status = v.Status.ToString().ToLower(),
                        LastDeploy = v.LastDeployDate?.ToString("yyyy-MM-dd")
                    }
                ),
                Checklist = component.ChecklistItems.OrderBy(ci => ci.Order).Select(ci => new ChecklistItemDto
                {
                    Id = ci.Id,
                    Item = ci.Description,
                    Completed = ci.IsCompleted,
                    Notes = ci.Notes
                }).ToList()
            };
        }
    }
}
