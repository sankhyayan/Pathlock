namespace TaskManager.API.Configuration
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    // Read allowed origins from configuration
                    var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>() 
                        ?? new[] { "http://localhost:5173" };
                    
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }
    }
}
