using System.Text.Json;
using TaskManager.API.Core.Entities;
using TaskManager.API.Core.Interfaces;

namespace TaskManager.API.Infrastructure.Persistence
{
    public class FileBasedTaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new();
        private readonly object _lock = new();
        private readonly ILogger<FileBasedTaskService> _logger;
        private readonly string _filePath;

        public FileBasedTaskService(ILogger<FileBasedTaskService> logger)
        {
            _logger = logger;
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "tasks.json");
            _logger.LogInformation("💾 FileBasedTaskService initialized");
            _logger.LogInformation("📁 Storage file: {FilePath}", _filePath);
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            lock (_lock)
            {
                try
                {
                    if (File.Exists(_filePath))
                    {
                        var json = File.ReadAllText(_filePath);
                        var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);
                        if (tasks != null)
                        {
                            _tasks.Clear();
                            _tasks.AddRange(tasks);
                            _logger.LogInformation("✅ Loaded {Count} tasks from file", _tasks.Count);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("📝 No existing tasks file found, starting fresh");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Failed to load tasks from file");
                }
            }
        }

        private void SaveToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(_tasks, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(_filePath, json);
                _logger.LogDebug("💾 Saved {Count} tasks to file", _tasks.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to save tasks to file");
            }
        }

        public Task<IEnumerable<TaskItem>> GetAllByProjectIdAsync(Guid projectId)
        {
            lock (_lock)
            {
                var tasks = _tasks.Where(t => t.ProjectId == projectId).ToList();
                _logger.LogDebug("📋 GetAllByProjectId({ProjectId}) - Current task count: {Count}", projectId, tasks.Count);
                return Task.FromResult<IEnumerable<TaskItem>>(tasks);
            }
        }

        public Task<TaskItem?> GetByIdAsync(Guid id)
        {
            lock (_lock)
            {
                var task = _tasks.FirstOrDefault(t => t.Id == id);
                _logger.LogDebug("🔎 GetById({Id}) - Found: {Found}", id, task != null);
                return Task.FromResult(task);
            }
        }

        public Task<TaskItem> CreateAsync(TaskItem task)
        {
            lock (_lock)
            {
                _tasks.Add(task);
                _logger.LogDebug("➕ Created task: ID={Id}, Title='{Title}', ProjectId={ProjectId}", 
                    task.Id, task.Title, task.ProjectId);
                SaveToFile();
                return Task.FromResult(task);
            }
        }

        public Task<TaskItem?> UpdateAsync(Guid id, TaskItem task)
        {
            lock (_lock)
            {
                var existingTask = _tasks.FirstOrDefault(t => t.Id == id);
                if (existingTask == null)
                {
                    _logger.LogDebug("❌ Update failed - Task not found: {Id}", id);
                    return Task.FromResult<TaskItem?>(null);
                }

                _logger.LogDebug("📝 Updating task {Id}: Completed: {OldStatus} -> {NewStatus}",
                    id, existingTask.Completed, task.Completed);

                existingTask.Title = task.Title;
                existingTask.Completed = task.Completed;
                existingTask.DueDate = task.DueDate;
                existingTask.EstimatedHours = task.EstimatedHours;
                existingTask.DependsOn = task.DependsOn ?? new List<Guid>();
                SaveToFile();
                return Task.FromResult<TaskItem?>(existingTask);
            }
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            lock (_lock)
            {
                var task = _tasks.FirstOrDefault(t => t.Id == id);
                if (task == null)
                {
                    _logger.LogDebug("❌ Delete failed - Task not found: {Id}", id);
                    return Task.FromResult(false);
                }

                _tasks.Remove(task);
                _logger.LogDebug("🗑️ Deleted task: ID={Id}, Title='{Title}', Remaining tasks={Count}",
                    task.Id, task.Title, _tasks.Count);
                SaveToFile();
                return Task.FromResult(true);
            }
        }

        public Task<bool> DeleteAllByProjectIdAsync(Guid projectId)
        {
            lock (_lock)
            {
                var tasksToDelete = _tasks.Where(t => t.ProjectId == projectId).ToList();
                if (tasksToDelete.Count == 0)
                {
                    return Task.FromResult(true);
                }

                foreach (var task in tasksToDelete)
                {
                    _tasks.Remove(task);
                }

                _logger.LogDebug("🗑️ Deleted {Count} tasks for project {ProjectId}", tasksToDelete.Count, projectId);
                SaveToFile();
                return Task.FromResult(true);
            }
        }
    }
}
