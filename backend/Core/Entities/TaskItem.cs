using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Core.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        public bool Completed { get; set; }
        
        public DateTime? DueDate { get; set; }

        // Optional estimate in hours for planning (e.g., 1.5 for 1h 30m)
        public double? EstimatedHours { get; set; }
        
        // Task IDs that this task depends on (must be completed first)
        public List<Guid> DependsOn { get; set; } = new List<Guid>();
        
        [Required]
        public Guid ProjectId { get; set; }
    }
}
