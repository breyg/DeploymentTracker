using GP.ADQ.DeploymentTracker.Domain.Entities;

namespace GP.ADQ.DeploymentTracker.Application.Interfaces
{
    public interface IComponentRepository
    {
        Task<Component?> GetByIdAsync(int id);
        Task<Component?> GetByIdWithDetailsAsync(int id);
        Task<Component?> GetByIdWithChecklistAsync(int id);
        Task<Component> AddAsync(Component component);
        Task<Component> UpdateAsync(Component component);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
