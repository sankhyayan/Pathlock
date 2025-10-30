using TaskManager.API.Configuration;
using TaskManager.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure application services
builder.Services.AddApplicationServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsPolicy();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

// app.UseHttpsRedirection(); // Commented out for development with HTTP only

// Global exception handling
app.UseExceptionHandlingMiddleware();

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
