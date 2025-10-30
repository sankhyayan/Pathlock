using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Application.DTOs;
using TaskManager.API.Application.Interfaces;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("🔐 Registration attempt for: {Username}", request.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("❌ Invalid registration data for: {Username}", request.Username);
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.RegisterAsync(request);
                _logger.LogInformation("✅ User registered successfully: {Username}", request.Username);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("⚠️ Registration failed for {Username}: {Message}", request.Username, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Registration error for: {Username}", request.Username);
                return StatusCode(500, new { message = "An error occurred during registration" });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("🔐 Login attempt for: {Email}", request.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("❌ Invalid login data for: {Email}", request.Email);
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.LoginAsync(request);
                _logger.LogInformation("✅ User logged in successfully: {Email}", request.Email);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("⚠️ Login failed for {Email}: {Message}", request.Email, ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Login error for: {Email}", request.Email);
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }
    }
}
