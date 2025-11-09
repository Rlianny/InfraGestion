using System.Security.Claims;
using Application.DTOs.Auth;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Web.API.Shared;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class UsersController : BaseApiController
    {
        private readonly IUserManagementService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserManagementService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
        {
            var administratorId = GetCurrentUserId();

            _logger.LogInformation(
                "Administrador {AdministratorId} intentando crear usuario: {Username}",
                administratorId,
                request.Username
            );

            var user = await _userService.CreateUserAsync(request, administratorId);

            _logger.LogInformation(
                "Usuario {UserId} creado exitosamente por administrador {AdministratorId}",
                user.UserId,
                administratorId
            );

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserById(int id)
        {
            _logger.LogInformation("Solicitando información de usuario {UserId}", id);

            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllUsers()
        {
            var currentUserId = GetCurrentUserId();

            _logger.LogInformation(
                "Usuario {UserId} solicitando lista de todos los usuarios activos",
                currentUserId
            );

            var users = await _userService.GetAllActiveUsersAsync();
            return Ok(users);
        }

        [HttpGet("department/{departmentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsersByDepartment(int departmentId)
        {
            var currentUserId = GetCurrentUserId();

            _logger.LogInformation(
                "Usuario {UserId} solicitando usuarios del departamento {DepartmentId}",
                currentUserId,
                departmentId
            );

            var users = await _userService.GetUsersByDepartmentAsync(departmentId);
            return Ok(users);
        }

        [HttpGet("role/{role}")]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            var currentUserId = GetCurrentUserId();

            _logger.LogInformation(
                "Usuario {UserId} solicitando usuarios con rol {Role}",
                currentUserId,
                role
            );

            var users = await _userService.GetUsersByRoleAsync(role);
            return Ok(users);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(
            int id,
            [FromBody] UpdateUserRequestDto request
        )
        {
            if (id != request.UserId)
            {
                return BadRequest("El Id de la URL no coincide con el Id del usuario en el body");
            }

            var administratorId = GetCurrentUserId();

            _logger.LogInformation(
                "Administrador {AdministratorId} intentando actualizar usuario {UserId}",
                administratorId,
                id
            );

            var user = await _userService.UpdateUserAsync(request, administratorId);

            _logger.LogInformation(
                "Usuario {UserId} actualizado exitosamente por administrador {AdministratorId}",
                user.UserId,
                administratorId
            );

            return Ok(user);
        }

        [HttpPost("{id}/deactivate")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateUser(
            int id,
            [FromBody] DeactivateUserRequestDto request
        )
        {
            if (id != request.UserId)
            {
                return BadRequest("El Id de la URL no coincide con el Id del usuario en el body");
            }

            var administratorId = GetCurrentUserId();

            _logger.LogInformation(
                "Administrador {AdministratorId} intentando desactivar usuario {UserId}",
                administratorId,
                id
            );

            await _userService.DeactivateUserAsync(request, administratorId);

            _logger.LogInformation(
                "Usuario {UserId} desactivado exitosamente por administrador {AdministratorId}",
                id,
                administratorId
            );

            return Ok("Succes");
        }

        [HttpPost("{id}/activate")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var administratorId = GetCurrentUserId();

            _logger.LogInformation(
                "Administrador {AdministratorId} intentando activar usuario {UserId}",
                administratorId,
                id
            );

            await _userService.ActivateUserAsync(id, administratorId);

            _logger.LogInformation(
                "Usuario {UserId} activado exitosamente por administrador {AdministratorId}",
                id,
                administratorId
            );

            return Ok("Succes");
        }

        [HttpGet("department/{departmentId}/technicians")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTechnicians(int departmentId)
        {
            var currentUserId = GetCurrentUserId();

            _logger.LogInformation(
                "Usuario {UserId} solicitando técnicos del departamento {DepartmentId}",
                currentUserId,
                departmentId
            );

            // Usar el método GetUsersByRoleAsync filtrando por departamento
            var allTechnicians = await _userService.GetUsersByRoleAsync("Technician");
            var departmentTechnicians = allTechnicians.Where(t => t.DepartmentId == departmentId);

            return Ok(departmentTechnicians);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogError("No se pudo obtener el Id del usuario del token JWT");
                throw new UnauthorizedAccessException("Token inválido");
            }

            return userId;
        }
    }
}
