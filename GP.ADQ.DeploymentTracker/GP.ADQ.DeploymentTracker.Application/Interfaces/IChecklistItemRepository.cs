using GP.ADQ.DeploymentTracker.Domain.Entities;

namespace GP.ADQ.DeploymentTracker.Domain.Interfaces
{
    public interface IChecklistItemRepository
    {
        Task<ChecklistItem?> GetByIdAsync(int id);
        Task<ChecklistItem> UpdateAsync(ChecklistItem item);
    }
}