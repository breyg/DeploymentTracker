using GP.ADQ.DeploymentTracker.Application.DTOs;

namespace GP.ADQ.DeploymentTracker.Application.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto request);
        Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto request);
        Task<bool> DeleteProjectAsync(int id);
    }
}
