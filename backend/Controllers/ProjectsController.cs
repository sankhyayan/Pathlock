using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Application.DTOs;
using TaskManager.API.Application.Mappers;
using TaskManager.API.Core.Interfaces;
using TaskManager.API.Controllers.Base;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : BaseApiController
    {
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(
            IProjectService projectService, 
            ITaskService taskService,
            ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation("📋 Getting projects for user: {UserId}", userId);

                var projects = await _projectService.GetAllByUserIdAsync(userId);
                return Ok(projects.ToDtoList());
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, "retrieving projects");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(Guid id)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation("🔎 Getting project {ProjectId} for user: {UserId}", id, userId);

                var project = await _projectService.GetByIdAsync(id);
                if (project == null)
                {
                    throw new KeyNotFoundException("Project not found");
                }

                if (project.UserId != userId)
                {
                    _logger.LogWarning("⚠️ User {UserId} attempted to access project {ProjectId} owned by {OwnerId}", 
                        userId, id, project.UserId);
                    throw new UnauthorizedAccessException("You don't have permission to access this project");
                }

                return Ok(project.ToDto());
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, "retrieving the project");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectRequest request)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation("➕ Creating project '{Title}' for user: {UserId}", request.Title, userId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var project = request.ToEntity(userId);
                var createdProject = await _projectService.CreateAsync(project);

                return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject.ToDto());
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, "creating the project");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation("🗑️ Deleting project {ProjectId} for user: {UserId}", id, userId);

                // Delete all tasks in the project first
                await _taskService.DeleteAllByProjectIdAsync(id);

                var deleted = await _projectService.DeleteAsync(id, userId);
                if (!deleted)
                {
                    throw new KeyNotFoundException("Project not found or you don't have permission to delete it");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, "deleting the project");
            }
        }
    }
}
