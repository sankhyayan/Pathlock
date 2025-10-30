using System.Text.Json;
using TaskManager.API.Core.Entities;
using TaskManager.API.Core.Interfaces;

namespace TaskManager.API.Infrastructure.Persistence
{
    public class FileBasedUserService : IUserService
    {
        private readonly List<User> _users = new();
        private readonly object _lock = new();
        private readonly ILogger<FileBasedUserService> _logger;
        private readonly string _filePath;

        public FileBasedUserService(ILogger<FileBasedUserService> logger)
        {
            _logger = logger;
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "users.json");
            _logger.LogInformation("💾 FileBasedUserService initialized");
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
                        var users = JsonSerializer.Deserialize<List<User>>(json);
                        if (users != null)
                        {
                            _users.Clear();
                            _users.AddRange(users);
                            _logger.LogInformation("✅ Loaded {Count} users from file", _users.Count);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("📝 No existing users file found, starting fresh");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Failed to load users from file");
                }
            }
        }

        private void SaveToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(_filePath, json);
                _logger.LogDebug("💾 Saved {Count} users to file", _users.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to save users to file");
            }
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            lock (_lock)
            {
                var user = _users.FirstOrDefault(u => u.Id == id);
                _logger.LogDebug("🔎 GetById({Id}) - Found: {Found}", id, user != null);
                return Task.FromResult(user);
            }
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            lock (_lock)
            {
                var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                _logger.LogDebug("🔎 GetByEmail({Email}) - Found: {Found}", email, user != null);
                return Task.FromResult(user);
            }
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            lock (_lock)
            {
                var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                _logger.LogDebug("🔎 GetByUsername({Username}) - Found: {Found}", username, user != null);
                return Task.FromResult(user);
            }
        }

        public Task<User> CreateAsync(User user)
        {
            lock (_lock)
            {
                _users.Add(user);
                _logger.LogDebug("➕ Created user: ID={Id}, Username='{Username}', Email='{Email}'", 
                    user.Id, user.Username, user.Email);
                SaveToFile();
                return Task.FromResult(user);
            }
        }
    }
}
