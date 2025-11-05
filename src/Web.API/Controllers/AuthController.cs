using Application.DTOs.Auth;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IUserManagementService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthenticationService authService,
            IUserManagementService userService,
            ILogger<AuthController> logger
        )
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                _logger.LogInformation(
                    "Usuario {Username} inició sesión exitosamente",
                    request.Username
                );
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(
                    "Intento de login fallido para {Username}: {Message}",
                    request.Username,
                    ex.Message
                );
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el login de {Username}", request.Username);
                return BadRequest(new { message = "Error durante el proceso de login" });
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken(
            [FromBody] RefreshTokenRequestDto request
        )
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(
                    request.UserId,
                    request.RefreshToken
                );
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Intento de refresh token inválido: {Message}", ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            try
            {
                var userIdClaim =
                    User.FindFirst("sub")?.Value
                    ?? User.FindFirst(
                        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                    )?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { message = "Token inválido" });
                }

                var user = await _userService.GetUserByIdAsync(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario actual");
                return BadRequest(new { message = "Error al obtener información del usuario" });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userIdClaim =
                    User.FindFirst("sub")?.Value
                    ?? User.FindFirst(
                        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                    )?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { message = "Token inválido" });
                }

                await _authService.LogoutAsync(userId);
                _logger.LogInformation("Usuario {UserId} cerró sesión", userId);
                return Ok(new { message = "Sesión cerrada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el logout");
                return BadRequest(new { message = "Error al cerrar sesión" });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            try
            {
                var userIdClaim =
                    User.FindFirst("sub")?.Value
                    ?? User.FindFirst(
                        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                    )?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { message = "Token inválido" });
                }

                // Asegurar que el userId del request coincide con el del token
                if (request.UserId != userId)
                {
                    return Forbid();
                }

                await _authService.ChangePasswordAsync(request);
                _logger.LogInformation("Usuario {UserId} cambió su contraseña", userId);
                return Ok(new { message = "Contraseña cambiada exitosamente" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar contraseña");
                return BadRequest(new { message = "Error al cambiar la contraseña" });
            }
        }
    }
}
