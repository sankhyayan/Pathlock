using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Application.DTOs;
using TaskManager.API.Application.Interfaces;
using TaskManager.API.Core.Interfaces;
using TaskManager.API.Controllers.Base;

namespace TaskManager.API.Controllers.Projects
{
    [Route("api/projects/{projectId}/schedule")]
    public class ProjectSchedulingController : BaseApiController
    {
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        private readonly ISchedulingService _schedulingService;
        private readonly ILogger<ProjectSchedulingController> _logger;

        public ProjectSchedulingController(
            IProjectService projectService, 
            ITaskService taskService,
            ISchedulingService schedulingService,
            ILogger<ProjectSchedulingController> logger)
        {
            _projectService = projectService;
            _taskService = taskService;
            _schedulingService = schedulingService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ScheduleResponse>> ScheduleTasks(Guid projectId)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation("🧠 Generating smart schedule for project {ProjectId}, user: {UserId}", 
                    projectId, userId);

                // Verify the project belongs to the user
                var project = await _projectService.GetByIdAsync(projectId);
                if (project == null)
                {
                    throw new KeyNotFoundException("Project not found");
                }

                if (project.UserId != userId)
                {
                    throw new UnauthorizedAccessException("You don't have permission to access this project");
                }

                // Get all tasks for the project
                var tasks = await _taskService.GetAllByProjectIdAsync(projectId);
                var taskList = tasks.ToList();

                if (taskList.Count == 0)
                {
                    return Ok(new ScheduleResponse());
                }

                // Generate schedule
                var schedule = _schedulingService.GenerateSchedule(taskList);
                
                _logger.LogInformation("✅ Generated schedule with {Count} tasks in recommended order", 
                    schedule.RecommendedOrder.Count);

                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, "generating the schedule");
            }
        }
    }
}
