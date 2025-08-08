using GP.ADQ.DeploymentTracker.Application.DTOs;
using GP.ADQ.DeploymentTracker.Application.Interfaces;
using GP.ADQ.DeploymentTracker.Domain.Entities;
using GP.ADQ.DeploymentTracker.Domain.Enums;

namespace GP.ADQ.DeploymentTracker.Application.Services
{
    public class ComponentService : IComponentService
    {
        private readonly IComponentRepository _componentRepository;
        private readonly IProjectRepository _projectRepository;

        public ComponentService(IComponentRepository componentRepository, IProjectRepository projectRepository)
        {
            _componentRepository = componentRepository;
            _projectRepository = projectRepository;
        }

        public async Task<ComponentDto?> GetComponentByIdAsync(int id)
        {
            var component = await _componentRepository.GetByIdWithDetailsAsync(id);
            return component == null ? null : MapToDto(component);
        }

        public async Task<ComponentDto> CreateComponentAsync(int projectId, CreateComponentDto request)
        {
            if (!await _projectRepository.ExistsAsync(projectId))
                throw new InvalidOperationException($"Project with ID {projectId} not found");

            var component = new Component()
            {
                Name = request.Name,
                Type = request.Type,
                JiraTicket = request.JiraTicket,
                ProjectId = projectId,
            };

            if (request.MemoryAllocation.HasValue)
                component.MemoryAllocation = request.MemoryAllocation.Value.ToString();

            if (request.Timeout.HasValue)
                component.Timeout = request.Timeout.Value.ToString();

            var createdComponent = await _componentRepository.AddAsync(component);
            return MapToDto(createdComponent);
        }

        public async Task<ComponentDto?> UpdateComponentAsync(int id, UpdateComponentDto request)
        {
            var component = await _componentRepository.GetByIdAsync(id);
            if (component == null) return null;

            component.UpdateInfo(
                request.Name,
                request.Type,
                request.JiraTicket
            );

            if (request.MemoryAllocation.HasValue)
                component.MemoryAllocation = request.MemoryAllocation.Value.ToString();

            if (request.Timeout.HasValue)
                component.Timeout = request.Timeout.Value.ToString();

            var updatedComponent = await _componentRepository.UpdateAsync(component);
            return MapToDto(updatedComponent);
        }

        public async Task<bool> DeleteComponentAsync(int id)
        {
            if (!await _componentRepository.ExistsAsync(id))
                return false;

            await _componentRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> UpdateComponentChecklistAsync(int componentId, UpdateChecklistDto request)
        {
            var component = await _componentRepository.GetByIdWithChecklistAsync(componentId);
            if (component == null) return false;

            // Limpiar checklist existente
            component.ChecklistItems.Clear();

            // Agregar nuevos items
            foreach (var item in request.Items)
            {
                component.AddChecklistItem(item.Description, item.Order);
                var addedItem = component.ChecklistItems.Last();
                if (item.IsCompleted)
                {
                    addedItem.MarkAsComplete();
                }
            }

            await _componentRepository.UpdateAsync(component);
            return true;
        }

        private static ComponentDto MapToDto(Component component)
        {
            return new ComponentDto
            {
                Id = component.Id,
                Name = component.Name,
                Type = component.Type.ToString(),
                JiraTicket = component.JiraTicket,
                MemoryAllocation = component.MemoryAllocation,
                Timeout = component.Timeout,
                Versions = component.Versions.ToDictionary(
                    v => v.Environment.ToString().ToLower(),
                    v => new VersionDto
                    {
                        Version = v.Version,
                        Status = v.Status.ToString().ToLower(),
                        LastDeploy = v.LastDeployDate?.ToString("yyyy-MM-dd")
                    }
                ),
                Checklist = component.ChecklistItems.OrderBy(ci => ci.Order).Select(ci => new ChecklistItemDto
                {
                    Id = ci.Id,
                    Item = ci.Description,
                    Completed = ci.IsCompleted,
                    Notes = ci.Notes
                }).ToList()
            };
        }

        public async Task<bool> UpdateComponentVersionAsync(int componentId, UpdateVersionDto request)
        {
            var component = await _componentRepository.GetByIdWithDetailsAsync(componentId);
            if (component == null) return false;

            var environment = Enum.Parse<EnvironmentType>(request.Environment, true);
            var status = Enum.Parse<DeploymentStatus>(request.Status, true);

            component.SetVersion(environment, request.Version, status);

            // Si se marca como deployed, actualizar datos de despliegue
            if (status == DeploymentStatus.Deployed)
            {
                var version = component.Versions.FirstOrDefault(v => v.Environment == environment);
                if (version != null)
                {
                    version.Deploy(request.DeployedBy ?? "system");
                }
            }

            await _componentRepository.UpdateAsync(component);
            return true;
        }

        public async Task<bool> DeployComponentAsync(int componentId, DeployComponentDto request)
        {
            var component = await _componentRepository.GetByIdWithDetailsAsync(componentId);
            if (component == null) return false;

            var environment = Enum.Parse<EnvironmentType>(request.Environment, true);

            // Marcar como desplegado
            component.SetVersion(environment, request.Version, DeploymentStatus.Deployed);

            var version = component.Versions.FirstOrDefault(v => v.Environment == environment);
            if (version != null)
            {
                version.Deploy(request.DeployedBy);
            }

            await _componentRepository.UpdateAsync(component);
            return true;
        }
    }
}