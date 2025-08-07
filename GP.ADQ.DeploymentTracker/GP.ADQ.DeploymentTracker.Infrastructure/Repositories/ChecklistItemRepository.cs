using GP.ADQ.DeploymentTracker.Domain.Entities;
using GP.ADQ.DeploymentTracker.Domain.Interfaces;
using GP.ADQ.DeploymentTracker.Infrastructure.Data;

namespace GP.ADQ.DeploymentTracker.Infrastructure.Repositories
{
    public class ChecklistItemRepository : IChecklistItemRepository
    {
        private readonly DeploymentTrackerContext _context;

        public ChecklistItemRepository(DeploymentTrackerContext context)
        {
            _context = context;
        }

        public async Task<ChecklistItem?> GetByIdAsync(int id)
        {
            return await _context.ChecklistItems.FindAsync(id);
        }

        public async Task<ChecklistItem> UpdateAsync(ChecklistItem item)
        {
            _context.ChecklistItems.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}