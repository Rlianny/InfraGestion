using Application.DTOs.Auth;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Shared;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("auth")]
    [Produces("application/json")]
    public class AuthController : BaseApiController
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
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
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
                return BadRequest(new { message = "Error durante el proceso de login: " + ex.Message });
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
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
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = GetCurrentUserId();
                var user = await _userService.GetUserByIdAsync(userId);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Intento de acceso no autorizado: {Message}", ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario actual");
                return BadRequest(new { message = "Error al obtener información del usuario" });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = GetCurrentUserId();
                await _authService.LogoutAsync(userId);
                _logger.LogInformation("Usuario {UserId} cerró sesión", userId);
                return Ok("Sesión cerrada exitosamente");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el logout");
                return BadRequest("Error al cerrar sesión");
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value);

            // if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            // {
            //     _logger.LogError("No se pudo obtener el claim 'sub' (UserID) del token JWT.");
            //     throw new UnauthorizedAccessException(
            //         "Token inválido o no contiene ID de usuario."
            //     );
            // }

            return userIdClaim;
        }
    }
}
