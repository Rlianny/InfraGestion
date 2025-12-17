using System.Security.Claims;
using Application.DTOs.Auth;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("administrator/{administratorId:int}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser(int administratorId, [FromBody] CreateUserRequestDto request)
        {
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

        [HttpGet("user/{currentUserId:int}")]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllUsers(int currentUserId)
        {
            _logger.LogInformation(
                "Usuario {UserId} solicitando lista de todos los usuarios activos",
                currentUserId
            );

            var activeUsers = await _userService.GetAllActiveUsersAsync();
            var unActiveUsers = await _userService.GetAllInactiveUsersAsync();

            return Ok(activeUsers.Concat(unActiveUsers));
        }

        [HttpGet("user/{currentUserId:int}/department/{departmentId:int}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsersByDepartment(int currentUserId, int departmentId)
        {
            _logger.LogInformation(
                "Usuario {UserId} solicitando usuarios del departamento {DepartmentId}",
                currentUserId,
                departmentId
            );

            var users = await _userService.GetUsersByDepartmentAsync(departmentId);
            return Ok(users);
        }

        [HttpGet("user/{currentUserId:int}/role/{role}")]
        [Authorize(Roles = "Administrator,Director,Logistician,SectionManager")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsersByRole(int currentUserId, string role)
        {
            _logger.LogInformation(
                "Usuario {UserId} solicitando usuarios con rol {Role}",
                currentUserId,
                role
            );

            var users = await _userService.GetUsersByRoleAsync(role);
            return Ok(users);
        }

        [HttpGet("director/dashboardInfo")]
        [Authorize(Roles = "Director")]
        [ProducesResponseType(typeof(ApiResponse<DashboardSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDashboardInfo()
        {
            var dashboardInfo = await _userService.GetDashboardInfoAsync();
            return Ok(dashboardInfo);
        }

        [HttpPut("administrator/{administratorId:int}/{id:int}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(int administratorId, int id, [FromBody] UpdateUserRequestDto request)
        {
            if (id != request.UserId)
            {
                return BadRequest("El Id de la URL no coincide con el Id del usuario en el body");
            }

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

        [HttpPost("administrator/{administratorId:int}/{id:int}/deactivate")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateUser(
            int administratorId,
            int id,
            [FromBody] DeactivateUserRequestDto request
        )
        {
            if (id != request.UserId)
            {
                return BadRequest("El Id de la URL no coincide con el Id del usuario en el body");
            }

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

        [HttpPost("administrator/{administratorId:int}/{id:int}/activate")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActivateUser(int administratorId, int id)
        {
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

        [HttpGet("user/{currentUserId:int}/department/{departmentId:int}/technicians")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTechnicians(int currentUserId, int departmentId)
        {
            _logger.LogInformation(
                "Usuario {UserId} solicitando técnicos del departamento {DepartmentId}",
                currentUserId,
                departmentId
            );

            var allTechnicians = await _userService.GetUsersByRoleAsync("Technician");
            var departmentTechnicians = allTechnicians.Where(t => t.DepartmentId == departmentId);

            return Ok(departmentTechnicians);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            try
            {
                await _userService.DeleteUserAync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
