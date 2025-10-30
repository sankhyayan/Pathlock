using TaskManager.API.Application.DTOs;
using TaskManager.API.Core.Entities;

namespace TaskManager.API.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        string GenerateJwtToken(User user);
    }
}
