using GP.ADQ.DeploymentTracker.Application.DTOs;
using GP.ADQ.DeploymentTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GP.ADQ.DeploymentTracker.API.Controllers
{
    [ApiController]
    [Route("api/checklist-items")]
    public class ChecklistController : ControllerBase
    {
        private readonly IChecklistService _checklistService;
        private readonly ILogger<ChecklistController> _logger;

        public ChecklistController(IChecklistService checklistService, ILogger<ChecklistController> logger)
        {
            _checklistService = checklistService;
            _logger = logger;
        }

        /// <summary>
        /// Toggle completion status of a checklist item
        /// </summary>
        [HttpPut("{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleChecklistItem(int itemId, [FromBody] ToggleChecklistItemDto request)
        {
            try
            {
                var success = await _checklistService.ToggleChecklistItemAsync(itemId, request.Completed);
                if (!success)
                    return NotFound($"Checklist item with ID {itemId} not found");

                return Ok(new { message = "Checklist item updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating checklist item {ItemId}", itemId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}