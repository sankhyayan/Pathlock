using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Models;
using TaskManager.API.Services;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        // GET: api/tasks
        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetAll()
        {
            _logger.LogInformation("🔍 GET /api/tasks - Fetching all tasks");
            var tasks = _taskService.GetAll();
            _logger.LogInformation("✅ Retrieved {Count} tasks", tasks.Count());
            return Ok(tasks);
        }

        // POST: api/tasks
        [HttpPost]
        public ActionResult<TaskItem> Create([FromBody] TaskItem task)
        {
            _logger.LogInformation("➕ POST /api/tasks - Creating task: {Description}", task.Description);
            
            if (string.IsNullOrWhiteSpace(task.Description))
            {
                _logger.LogWarning("⚠️ Validation failed: Description is empty");
                return BadRequest("Description is required");
            }

            var createdTask = _taskService.Create(task);
            _logger.LogInformation("✅ Task created with ID: {Id}", createdTask.Id);
            return Ok(createdTask);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public ActionResult<TaskItem> Update(Guid id, [FromBody] TaskItem task)
        {
            _logger.LogInformation("📝 PUT /api/tasks/{Id} - Updating task", id);
            _logger.LogInformation("   Description: {Description}, IsCompleted: {IsCompleted}", 
                task.Description, task.IsCompleted);
            
            if (string.IsNullOrWhiteSpace(task.Description))
            {
                _logger.LogWarning("⚠️ Validation failed: Description is empty");
                return BadRequest("Description is required");
            }

            var updatedTask = _taskService.Update(id, task);
            if (updatedTask == null)
            {
                _logger.LogWarning("❌ Task not found: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("✅ Task updated successfully");
            return Ok(updatedTask);
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            _logger.LogInformation("🗑️ DELETE /api/tasks/{Id}", id);
            
            var success = _taskService.Delete(id);
            if (!success)
            {
                _logger.LogWarning("❌ Task not found: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("✅ Task deleted successfully");
            return NoContent();
        }
    }
}
