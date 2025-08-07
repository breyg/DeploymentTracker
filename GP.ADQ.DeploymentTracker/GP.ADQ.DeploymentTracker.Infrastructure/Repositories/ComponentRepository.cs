using GP.ADQ.DeploymentTracker.Application.Interfaces;
using GP.ADQ.DeploymentTracker.Domain.Entities;
using GP.ADQ.DeploymentTracker.Domain.Interfaces;
using GP.ADQ.DeploymentTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GP.ADQ.DeploymentTracker.Infrastructure.Repositories
{
    public class ComponentRepository : IComponentRepository
    {
        private readonly DeploymentTrackerContext _context;

        public ComponentRepository(DeploymentTrackerContext context)
        {
            _context = context;
        }

        public async Task<Component?> GetByIdAsync(int id)
        {
            return await _context.Components.FindAsync(id);
        }

        public async Task<Component?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Components
                .Include(c => c.Versions)
                .Include(c => c.ChecklistItems.OrderBy(ci => ci.Order))
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Component?> GetByIdWithChecklistAsync(int id)
        {
            return await _context.Components
                .Include(c => c.ChecklistItems)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Component> AddAsync(Component component)
        {
            _context.Components.Add(component);
            await _context.SaveChangesAsync();
            return component;
        }

        public async Task<Component> UpdateAsync(Component component)
        {
            _context.Components.Update(component);
            await _context.SaveChangesAsync();
            return await GetByIdWithDetailsAsync(component.Id) ?? component;
        }

        public async Task DeleteAsync(int id)
        {
            var component = await GetByIdAsync(id);
            if (component != null)
            {
                _context.Components.Remove(component);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Components.AnyAsync(c => c.Id == id);
        }
    }
}