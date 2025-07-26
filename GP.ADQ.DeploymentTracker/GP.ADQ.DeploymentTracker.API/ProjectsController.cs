using GP.ADQ.DeploymentTracker.Application.DTOs;
using GP.ADQ.DeploymentTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GP.ADQ.DeploymentTracker.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        /// <summary>
        /// Get all projects with their components
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProjectDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProjectDto>>> GetAllProjects()
        {
            try
            {
                var projects = await _projectService.GetAllProjectsAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all projects");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get a specific project by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            try
            {
                var project = await _projectService.GetProjectByIdAsync(id);
                if (project == null)
                    return NotFound($"Project with ID {id} not found");

                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project {ProjectId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new project
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var project = await _projectService.CreateProjectAsync(createDto);
                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update an existing project
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectDto>> UpdateProject(int id, UpdateProjectDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var project = await _projectService.UpdateProjectAsync(id, updateDto);
                if (project == null)
                    return NotFound($"Project with ID {id} not found");

                return Ok(project);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project {ProjectId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProject(int id)
        {
            try
            {
                var success = await _projectService.DeleteProjectAsync(id);
                if (!success)
                    return NotFound($"Project with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project {ProjectId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
