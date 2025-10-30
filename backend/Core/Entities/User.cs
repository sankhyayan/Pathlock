using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        
        [Required]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
    }
}
