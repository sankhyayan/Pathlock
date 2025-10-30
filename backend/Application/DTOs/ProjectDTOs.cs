using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Application.DTOs
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateProjectRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string? Description { get; set; }
    }
}
