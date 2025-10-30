using TaskManager.API.Application.DTOs;
using TaskManager.API.Core.Entities;

namespace TaskManager.API.Application.Mappers
{
    public static class TaskMapper
    {
        public static TaskDto ToDto(this TaskItem task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Completed = task.Completed,
                DueDate = task.DueDate,
                EstimatedHours = task.EstimatedHours,
                DependsOn = task.DependsOn ?? new List<Guid>(),
                ProjectId = task.ProjectId
            };
        }

        public static IEnumerable<TaskDto> ToDtoList(this IEnumerable<TaskItem> tasks)
        {
            return tasks.Select(t => t.ToDto());
        }

        public static TaskItem ToEntity(this CreateTaskRequest request, Guid projectId)
        {
            return new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Completed = false,
                DueDate = request.DueDate,
                EstimatedHours = request.EstimatedHours,
                DependsOn = request.DependsOn ?? new List<Guid>(),
                ProjectId = projectId
            };
        }

        public static void UpdateEntity(this TaskItem task, UpdateTaskRequest request)
        {
            task.Title = request.Title;
            task.Completed = request.Completed;
            task.DueDate = request.DueDate;
            task.EstimatedHours = request.EstimatedHours;
            task.DependsOn = request.DependsOn ?? new List<Guid>();
        }
    }
}
