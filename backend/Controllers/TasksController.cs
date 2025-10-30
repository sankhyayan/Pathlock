using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Application.DTOs;
using TaskManager.API.Application.Mappers;
using TaskManager.API.Core.Interfaces;
using TaskManager.API.Controllers.Base;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : BaseApiController
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(
            ITaskService taskService,
            IProjectService projectService,
            ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _projectService = projectService;
            _logger = logger;
        }

        private async Task<bool> VerifyTaskAccess(Guid taskId, Guid userId)
        {
            var task = await _taskService.GetByIdAsync(taskId);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found");
            }

            var project = await _projectService.GetByIdAsync(task.ProjectId);
            if (project == null || project.UserId != userId)
            {
                _logger.LogWarning("⚠️ User {UserId} attempted to access task {TaskId} in project {ProjectId}",
                    userId, taskId, task.ProjectId);
                throw new UnauthorizedAccessException("You don't have permission to access this task");
            }

            return true;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskDto>> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation("📝 Updating task {TaskId} for user: {UserId}", id, userId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await VerifyTaskAccess(id, userId);

                var existingTask = await _taskService.GetByIdAsync(id);
                    if (existingTask == null)
                    {
                        throw new KeyNotFoundException("Task not found");
                    }

                    existingTask.UpdateEntity(request);

                var updatedTask = await _taskService.UpdateAsync(id, existingTask);
                if (updatedTask == null)
                {
                    throw new KeyNotFoundException("Task not found");
                }

                return Ok(updatedTask.ToDto());
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, "updating the task");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation("🗑️ Deleting task {TaskId} for user: {UserId}", id, userId);

                await VerifyTaskAccess(id, userId);

                var deleted = await _taskService.DeleteAsync(id);
                if (!deleted)
                {
                    throw new KeyNotFoundException("Task not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, "deleting the task");
            }
        }
    }
}
