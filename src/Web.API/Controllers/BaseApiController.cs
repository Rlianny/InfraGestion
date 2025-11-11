using Microsoft.AspNetCore.Mvc;
using Web.API.Shared;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
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