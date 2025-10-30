using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Application.DTOs;
using TaskManager.API.Application.Mappers;
using TaskManager.API.Core.Interfaces;
using TaskManager.API.Controllers.Base;

namespace TaskManager.API.Controllers.Projects
{
    [Route("api/projects/{projectId}/tasks")]
    public class ProjectTasksController : BaseApiController
    {
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        private readonly ILogger<ProjectTasksController> _logger;

        public ProjectTasksController(
            IProjectService projectService, 
            ITaskService taskService,
            ILogger<ProjectTasksController> logger)
        {
            _projectService = projectService;
            _taskService = taskService;
            _logger = logger;
        }

        private async Task<bool> VerifyProjectAccess(Guid projectId, Guid userId)
        {
            var project = await _projectService.GetByIdAsync(projectId);
            if (project == null)
            {
                throw new KeyNotFoundException("Project not found");
            }

            if (project.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't have permission to access this project");
            }

            return true;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetProjectTasks(Guid projectId)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation("📋 Getting tasks for project {ProjectId}, user: {UserId}", projectId, userId);

                await VerifyProjectAccess(projectId, userId);

                var tasks = await _taskService.GetAllByProjectIdAsync(projectId);
                return Ok(tasks.ToDtoList());
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, "retrieving tasks");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(Guid projectId, [FromBody] CreateTaskRequest request)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation("➕ Creating task '{Title}' for project {ProjectId}, user: {UserId}", 
                    request.Title, projectId, userId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await VerifyProjectAccess(projectId, userId);

                var task = request.ToEntity(projectId);
                var createdTask = await _taskService.CreateAsync(task);

                return CreatedAtAction(nameof(GetProjectTasks), new { projectId }, createdTask.ToDto());
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, "creating the task");
            }
        }
    }
}
