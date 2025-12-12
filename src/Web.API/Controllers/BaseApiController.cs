using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Web.API.Shared;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected int GetCurrentUserId()
        {
            var userIdClaim =
                User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        protected string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }


        protected IActionResult Ok<T>(T data, string message = null)
        {
            return base.Ok(ApiResponse<T>.Ok(data, message));
        }

        protected IActionResult Ok(string message = null)
        {
            return base.Ok(ApiResponse<string?>.Ok(message));
        }

        protected IActionResult BadRequest(string error, string message = null)
        {
            return base.BadRequest(ApiResponse<string?>.Fail(error, message));
        }

        protected IActionResult BadRequest(List<string> errors, string message = null)
        {
            return base.BadRequest(ApiResponse<string>.Fail(errors, message));
        }

        protected IActionResult NotFound(string message = "Resource not found")
        {
            return base.NotFound(ApiResponse<string?>.Fail("Not found", message));
        }
    }
}
