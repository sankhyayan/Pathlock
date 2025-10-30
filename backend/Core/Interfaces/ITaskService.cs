using TaskManager.API.Core.Entities;

namespace TaskManager.API.Core.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllByProjectIdAsync(Guid projectId);
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<TaskItem?> UpdateAsync(Guid id, TaskItem task);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllByProjectIdAsync(Guid projectId);
    }
}
