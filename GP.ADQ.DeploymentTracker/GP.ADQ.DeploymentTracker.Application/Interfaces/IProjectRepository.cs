using GP.ADQ.DeploymentTracker.Domain.Entities;

namespace GP.ADQ.DeploymentTracker.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
        Task<Project?> GetByIdWithComponentsAsync(int id);
        Task<Project> AddAsync(Project project);
        Task<Project> UpdateAsync(Project project);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
    }
}
