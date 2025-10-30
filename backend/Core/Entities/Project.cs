using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Core.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
    }
}
