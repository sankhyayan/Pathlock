using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Application.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public DateTime? DueDate { get; set; }
        public double? EstimatedHours { get; set; }
        public List<Guid> DependsOn { get; set; } = new List<Guid>();
        public Guid ProjectId { get; set; }
    }

    public class CreateTaskRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;
        
        public DateTime? DueDate { get; set; }
        [Range(0, 10000)]
        public double? EstimatedHours { get; set; }
        
        public List<Guid> DependsOn { get; set; } = new List<Guid>();
    }

    public class UpdateTaskRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;
        
        public bool Completed { get; set; }
        
        public DateTime? DueDate { get; set; }

        [Range(0, 10000)]
        public double? EstimatedHours { get; set; }
        
        public List<Guid> DependsOn { get; set; } = new List<Guid>();
    }

    public class ScheduleResponse
    {
        public List<Guid> RecommendedOrder { get; set; } = new List<Guid>();
        public List<string> RecommendedTitles { get; set; } = new List<string>();
    }
}
