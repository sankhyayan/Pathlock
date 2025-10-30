using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Application.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = null!;
    }

    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
