using TaskManager.API.Models;
using System.Text.Json;

namespace TaskManager.API.Services
{
    public interface ITaskService
    {
        IEnumerable<TaskItem> GetAll();
        TaskItem? GetById(Guid id);
        TaskItem Create(TaskItem task);
        TaskItem? Update(Guid id, TaskItem task);
        bool Delete(Guid id);
    }

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

        public IEnumerable<TaskItem> GetAll()
        {
            lock (_lock)
            {
                _logger.LogDebug("📋 GetAll() - Current task count: {Count}", _tasks.Count);
                return _tasks.ToList();
            }
        }

        public TaskItem? GetById(Guid id)
        {
            lock (_lock)
            {
                var task = _tasks.FirstOrDefault(t => t.Id == id);
                _logger.LogDebug("🔎 GetById({Id}) - Found: {Found}", id, task != null);
                return task;
            }
        }

        public TaskItem Create(TaskItem task)
        {
            lock (_lock)
            {
                task.Id = Guid.NewGuid();
                _tasks.Add(task);
                _logger.LogDebug("➕ Created task: ID={Id}, Description='{Description}', Total tasks={Count}", 
                    task.Id, task.Description, _tasks.Count);
                SaveToFile();
                return task;
            }
        }

        public TaskItem? Update(Guid id, TaskItem task)
        {
            lock (_lock)
            {
                var existingTask = _tasks.FirstOrDefault(t => t.Id == id);
                if (existingTask == null)
                {
                    _logger.LogDebug("❌ Update failed - Task not found: {Id}", id);
                    return null;
                }

                _logger.LogDebug("📝 Updating task {Id}: '{OldDesc}' -> '{NewDesc}', Completed: {OldStatus} -> {NewStatus}",
                    id, existingTask.Description, task.Description, 
                    existingTask.IsCompleted, task.IsCompleted);

                existingTask.Description = task.Description;
                existingTask.IsCompleted = task.IsCompleted;
                SaveToFile();
                return existingTask;
            }
        }

        public bool Delete(Guid id)
        {
            lock (_lock)
            {
                var task = _tasks.FirstOrDefault(t => t.Id == id);
                if (task == null)
                {
                    _logger.LogDebug("❌ Delete failed - Task not found: {Id}", id);
                    return false;
                }

                _tasks.Remove(task);
                _logger.LogDebug("🗑️ Deleted task: ID={Id}, Description='{Description}', Remaining tasks={Count}",
                    task.Id, task.Description, _tasks.Count);
                SaveToFile();
                return true;
            }
        }
    }
}
