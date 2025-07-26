using GP.ADQ.DeploymentTracker.Application.Interfaces;
using GP.ADQ.DeploymentTracker.Domain.Entities;
using GP.ADQ.DeploymentTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GP.ADQ.DeploymentTracker.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DeploymentTrackerContext _context;

        public ProjectRepository(DeploymentTrackerContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Components)
                    .ThenInclude(c => c.Versions)
                .Include(p => p.Components)
                    .ThenInclude(c => c.ChecklistItems)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project?> GetByIdWithComponentsAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Components)
                    .ThenInclude(c => c.Versions)
                .Include(p => p.Components)
                    .ThenInclude(c => c.ChecklistItems)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> AddAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Projects.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            var query = _context.Projects.Where(p => p.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}
