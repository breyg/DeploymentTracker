using GP.ADQ.DeploymentTracker.Application.Interfaces;
using GP.ADQ.DeploymentTracker.Domain.Entities;
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

        public async Task<List<Component>> GetByProjectIdAsync(int projectId)
        {
            return await _context.Components
                .Include(c => c.Versions)
                .Include(c => c.ChecklistItems)
                .Where(c => c.ProjectId == projectId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Component?> GetByIdAsync(int id)
        {
            return await _context.Components
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Component?> GetByIdWithVersionsAsync(int id)
        {
            return await _context.Components
                .Include(c => c.Versions)
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
            return component;
        }

        public async Task DeleteAsync(int id)
        {
            var component = await _context.Components.FindAsync(id);
            if (component != null)
            {
                _context.Components.Remove(component);
                await _context.SaveChangesAsync();
            }
        }
    }
}
