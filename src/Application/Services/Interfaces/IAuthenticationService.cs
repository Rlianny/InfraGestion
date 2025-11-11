using Application.DTOs.Auth;

namespace Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
        Task LogoutAsync(int userId, CancellationToken cancellationToken = default);
        Task ChangePasswordAsync(ChangePasswordRequestDto request, CancellationToken cancellationToken = default);
        Task<bool> ValidateUserRoleAsync(int userId, string requiredRole, CancellationToken cancellationToken = default);
        Task<bool> CanAccessDepartmentAsync(int userId, int departmentId, CancellationToken cancellationToken = default);
        Task<LoginResponseDto> RefreshTokenAsync(int userId, string refreshToken, CancellationToken cancellationToken = default);
    }
}
