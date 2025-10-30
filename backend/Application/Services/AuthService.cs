using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskManager.API.Core.Entities;
using TaskManager.API.Core.Interfaces;
using TaskManager.API.Application.DTOs;
using TaskManager.API.Application.Interfaces;

namespace TaskManager.API.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserService userService, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            _logger.LogInformation("🔐 Registering new user: {Email}", request.Email);

            // Check if user already exists
            var existingUser = await _userService.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("⚠️ Registration failed - Email already exists: {Email}", request.Email);
                throw new InvalidOperationException("Email already registered");
            }

            // Check username
            var existingUsername = await _userService.GetByUsernameAsync(request.Username);
            if (existingUsername != null)
            {
                _logger.LogWarning("⚠️ Registration failed - Username already exists: {Username}", request.Username);
                throw new InvalidOperationException("Username already taken");
            }

            // Create user
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _userService.CreateAsync(user);

            _logger.LogInformation("✅ User registered successfully: {Email}", request.Email);

            var token = GenerateJwtToken(user);
            return new AuthResponse
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id.ToString(),
                    Username = user.Username,
                    Email = user.Email
                }
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            _logger.LogInformation("🔑 Login attempt: {Email}", request.Email);

            var user = await _userService.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("❌ Login failed - Invalid credentials: {Email}", request.Email);
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            _logger.LogInformation("✅ Login successful: {Email}", request.Email);

            var token = GenerateJwtToken(user);
            return new AuthResponse
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id.ToString(),
                    Username = user.Username,
                    Email = user.Email
                }
            };
        }

        public string GenerateJwtToken(User user)
        {
            var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(double.Parse(_configuration["Jwt:ExpiryInHours"] ?? "24")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
