using GP.ADQ.DeploymentTracker.Application.DTOs;

namespace GP.ADQ.DeploymentTracker.Application.Interfaces
{
    public interface IComponentService
    {
        Task<ComponentDto?> GetComponentByIdAsync(int id);
        Task<ComponentDto> CreateComponentAsync(int projectId, CreateComponentDto request);
        Task<ComponentDto?> UpdateComponentAsync(int id, UpdateComponentDto request);
        Task<bool> DeleteComponentAsync(int id);
        Task<bool> UpdateComponentChecklistAsync(int componentId, UpdateChecklistDto request);
        Task<bool> UpdateComponentVersionAsync(int componentId, UpdateVersionDto request);
        Task<bool> DeployComponentAsync(int componentId, DeployComponentDto request);
    }
}