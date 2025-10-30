using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskManager.API.Controllers.Base
{
    [Authorize]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid user token");
            }
            return userId;
        }

        protected ActionResult HandleException(Exception ex, ILogger logger, string operation)
        {
            return ex switch
            {
                UnauthorizedAccessException => Unauthorized(new { message = ex.Message }),
                KeyNotFoundException => NotFound(new { message = ex.Message }),
                ArgumentException => BadRequest(new { message = ex.Message }),
                InvalidOperationException => BadRequest(new { message = ex.Message }),
                _ => HandleGenericException(ex, logger, operation)
            };
        }

        private ActionResult HandleGenericException(Exception ex, ILogger logger, string operation)
        {
            logger.LogError(ex, "❌ Error during {Operation}", operation);
            return StatusCode(500, new { message = $"An error occurred while {operation}" });
        }
    }
}
