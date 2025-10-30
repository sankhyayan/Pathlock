using TaskManager.API.Core.Entities;

namespace TaskManager.API.Core.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(Guid id);
        Task<User> CreateAsync(User user);
    }
}
