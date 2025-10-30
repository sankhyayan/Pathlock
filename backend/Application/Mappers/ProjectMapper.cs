using TaskManager.API.Application.DTOs;
using TaskManager.API.Core.Entities;

namespace TaskManager.API.Application.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectDto ToDto(this Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
        }

        public static IEnumerable<ProjectDto> ToDtoList(this IEnumerable<Project> projects)
        {
            return projects.Select(p => p.ToDto());
        }

        public static Project ToEntity(this CreateProjectRequest request, Guid userId)
        {
            return new Project
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
        }
    }
}
