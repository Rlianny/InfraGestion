using Application.DTOs.Auth;

namespace Application.Services.Interfaces
{
    public interface IUserManagementService
    {
        Task<UserDto> CreateUserAsync(
            CreateUserRequestDto request,
            int administratorId,
            CancellationToken cancellationToken = default
        );

        Task<UserDto> UpdateUserAsync(
            UpdateUserRequestDto request,
            int administratorId,
            CancellationToken cancellationToken = default
        );

        Task DeactivateUserAsync(
            DeactivateUserRequestDto request,
            int administratorId,
            CancellationToken cancellationToken = default
        );

        Task ActivateUserAsync(
            int userId,
            int administratorId,
            CancellationToken cancellationToken = default
        );

        Task<UserDto> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserDto>> GetAllUsersAsync(
            CancellationToken cancellationToken = default
        );

        Task<IEnumerable<UserDto>> GetAllActiveUsersAsync(
            CancellationToken cancellationToken = default
        );

        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(
            string roleName,
            CancellationToken cancellationToken = default
        );

        Task<IEnumerable<UserDto>> GetUsersByDepartmentAsync(
            int departmentId,
            CancellationToken cancellationToken = default
        );
    }
}
