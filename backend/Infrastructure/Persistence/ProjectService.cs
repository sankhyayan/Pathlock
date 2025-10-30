using System.Text.Json;
using TaskManager.API.Core.Entities;
using TaskManager.API.Core.Interfaces;

namespace TaskManager.API.Infrastructure.Persistence
{
    public class FileBasedProjectService : IProjectService
    {
        private readonly List<Project> _projects = new();
        private readonly object _lock = new();
        private readonly ILogger<FileBasedProjectService> _logger;
        private readonly string _filePath;

        public FileBasedProjectService(ILogger<FileBasedProjectService> logger)
        {
            _logger = logger;
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "projects.json");
            _logger.LogInformation("💾 FileBasedProjectService initialized");
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
                        var projects = JsonSerializer.Deserialize<List<Project>>(json);
                        if (projects != null)
                        {
                            _projects.Clear();
                            _projects.AddRange(projects);
                            _logger.LogInformation("✅ Loaded {Count} projects from file", _projects.Count);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("📝 No existing projects file found, starting fresh");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Failed to load projects from file");
                }
            }
        }

        private void SaveToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(_projects, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(_filePath, json);
                _logger.LogDebug("💾 Saved {Count} projects to file", _projects.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to save projects to file");
            }
        }

        public Task<IEnumerable<Project>> GetAllByUserIdAsync(Guid userId)
        {
            lock (_lock)
            {
                var projects = _projects.Where(p => p.UserId == userId).ToList();
                _logger.LogDebug("📋 GetAllByUserId({UserId}) - Count: {Count}", userId, projects.Count);
                return Task.FromResult<IEnumerable<Project>>(projects);
            }
        }

        public Task<Project?> GetByIdAsync(Guid id)
        {
            lock (_lock)
            {
                var project = _projects.FirstOrDefault(p => p.Id == id);
                _logger.LogDebug("🔎 GetById({Id}) - Found: {Found}", id, project != null);
                return Task.FromResult(project);
            }
        }

        public Task<Project> CreateAsync(Project project)
        {
            lock (_lock)
            {
                _projects.Add(project);
                _logger.LogDebug("➕ Created project: ID={Id}, Title='{Title}', UserId={UserId}", 
                    project.Id, project.Title, project.UserId);
                SaveToFile();
                return Task.FromResult(project);
            }
        }

        public Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            lock (_lock)
            {
                var project = _projects.FirstOrDefault(p => p.Id == id && p.UserId == userId);
                if (project == null)
                {
                    _logger.LogDebug("❌ Delete failed - Project not found or unauthorized: {Id}", id);
                    return Task.FromResult(false);
                }

                _projects.Remove(project);
                _logger.LogDebug("🗑️ Deleted project: ID={Id}, Title='{Title}'", project.Id, project.Title);
                SaveToFile();
                return Task.FromResult(true);
            }
        }
    }
}
