using GP.ADQ.DeploymentTracker.Application.Interfaces;
using GP.ADQ.DeploymentTracker.Domain.Interfaces;

namespace GP.ADQ.DeploymentTracker.Application.Services
{
    public class ChecklistService : IChecklistService
    {
        private readonly IChecklistItemRepository _checklistItemRepository;

        public ChecklistService(IChecklistItemRepository checklistItemRepository)
        {
            _checklistItemRepository = checklistItemRepository;
        }

        public async Task<bool> ToggleChecklistItemAsync(int itemId, bool completed)
        {
            var item = await _checklistItemRepository.GetByIdAsync(itemId);
            if (item == null) return false;

            if (completed)
                item.MarkAsComplete();
            else
                item.MarkAsIncomplete();

            await _checklistItemRepository.UpdateAsync(item);
            return true;
        }
    }
}