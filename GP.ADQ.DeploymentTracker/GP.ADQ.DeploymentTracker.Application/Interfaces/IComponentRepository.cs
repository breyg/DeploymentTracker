using GP.ADQ.DeploymentTracker.Domain.Entities;

namespace GP.ADQ.DeploymentTracker.Application.Interfaces
{
    public interface IComponentRepository
    {
        Task<List<Component>> GetByProjectIdAsync(int projectId);
        Task<Component?> GetByIdAsync(int id);
        Task<Component?> GetByIdWithVersionsAsync(int id);
        Task<Component> AddAsync(Component component);
        Task<Component> UpdateAsync(Component component);
        Task DeleteAsync(int id);
    }
}
