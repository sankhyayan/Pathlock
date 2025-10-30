using TaskManager.API.Application.DTOs;
using TaskManager.API.Core.Entities;

namespace TaskManager.API.Application.Interfaces
{
    public interface ISchedulingService
    {
        ScheduleResponse GenerateSchedule(List<TaskItem> tasks);
    }
}
