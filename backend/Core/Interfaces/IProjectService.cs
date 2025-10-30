using TaskManager.API.Core.Entities;

namespace TaskManager.API.Core.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllByUserIdAsync(Guid userId);
        Task<Project?> GetByIdAsync(Guid id);
        Task<Project> CreateAsync(Project project);
        Task<bool> DeleteAsync(Guid id, Guid userId);
    }
}
