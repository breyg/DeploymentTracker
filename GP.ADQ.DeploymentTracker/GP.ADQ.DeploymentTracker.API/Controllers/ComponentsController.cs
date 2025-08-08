using GP.ADQ.DeploymentTracker.Application.DTOs;
using GP.ADQ.DeploymentTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GP.ADQ.DeploymentTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentsController : ControllerBase
    {
        private readonly IComponentService _componentService;
        private readonly ILogger<ComponentsController> _logger;

        public ComponentsController(IComponentService componentService, ILogger<ComponentsController> logger)
        {
            _componentService = componentService;
            _logger = logger;
        }

        /// <summary>
        /// Get a specific component by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ComponentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComponentDto>> GetComponent(int id)
        {
            try
            {
                var component = await _componentService.GetComponentByIdAsync(id);
                if (component == null)
                    return NotFound($"Component with ID {id} not found");

                return Ok(component);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving component {ComponentId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new component in a project
        /// </summary>
        [HttpPost("projects/{projectId}")]
        [ProducesResponseType(typeof(ComponentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ComponentDto>> CreateComponent(int projectId, [FromBody] CreateComponentDto request)
        {
            try
            {
                var component = await _componentService.CreateComponentAsync(projectId, request);
                return CreatedAtAction(nameof(GetComponent), new { id = component.Id }, component);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating component in project {ProjectId}", projectId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update a component
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ComponentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComponentDto>> UpdateComponent(int id, [FromBody] UpdateComponentDto request)
        {
            try
            {
                var component = await _componentService.UpdateComponentAsync(id, request);
                if (component == null)
                    return NotFound($"Component with ID {id} not found");

                return Ok(component);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating component {ComponentId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a component
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteComponent(int id)
        {
            try
            {
                var success = await _componentService.DeleteComponentAsync(id);
                if (!success)
                    return NotFound($"Component with ID {id} not found");

                return Ok(new { message = "Component deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting component {ComponentId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update entire checklist for a component
        /// </summary>
        [HttpPut("{componentId}/checklist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateComponentChecklist(int componentId, [FromBody] UpdateChecklistDto request)
        {
            try
            {
                var success = await _componentService.UpdateComponentChecklistAsync(componentId, request);
                if (!success)
                    return NotFound($"Component with ID {componentId} not found");

                return Ok(new { message = "Checklist updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating checklist for component {ComponentId}", componentId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update version for a specific environment
        /// </summary>
        [HttpPut("{componentId}/versions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateComponentVersion(int componentId, [FromBody] UpdateVersionDto request)
        {
            try
            {
                var success = await _componentService.UpdateComponentVersionAsync(componentId, request);
                if (!success)
                    return NotFound($"Component with ID {componentId} not found");

                return Ok(new { message = "Version updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating version for component {ComponentId}", componentId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Quick deploy to environment
        /// </summary>
        [HttpPost("{componentId}/deploy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeployComponent(int componentId, [FromBody] DeployComponentDto request)
        {
            try
            {
                var success = await _componentService.DeployComponentAsync(componentId, request);
                if (!success)
                    return NotFound($"Component with ID {componentId} not found");

                return Ok(new { message = "Component deployed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deploying component {ComponentId}", componentId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}