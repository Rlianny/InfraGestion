using Application.DTOs.Auth;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationService> _logger;

        // These will be implemented in Infrastructure layer
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper,
            ILogger<AuthenticationService> logger
        )
        {
            _userRepository =
                userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher =
                passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _jwtTokenGenerator =
                jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Attempting login for identifier: {Identifier}",
                request.Username
            );

            var user = await _userRepository.GetByUserNameAsync(
                request.Username,
                cancellationToken
            );

            if (user == null)
            {
                _logger.LogWarning(
                    "Login failed: User not found for identifier: {Identifier}",
                    request.Username
                );
                throw new AuthenticationException("Credenciales inválidas");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Login failed: User account is inactive: {UserId}", user.UserID);
                throw new AuthenticationException("La cuenta de usuario está inactiva");
            }

            // Verify password
            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning(
                    "Login failed: Invalid password for user: {UserId}",
                    user.UserID
                );
                throw new AuthenticationException("Credenciales inválidas");
            }

            // Generate JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddMinutes(30); // Token valid for 30 minutes
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7); // Refresh token valid for 7 days

            // Store refresh token
            user.SetRefreshToken(refreshToken, refreshTokenExpiry);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Map to response DTO
            var response = _mapper.Map<LoginResponseDto>(user);

            _logger.LogInformation(
                "Login successful for user: {UserId}, Role: {Role}",
                user.UserID,
                user.Role.Name
            );

            return response;
        }

        public async Task LogoutAsync(int userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Logging out user: {UserId}", userId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Logout failed: User not found: {UserId}", userId);
                throw new EntityNotFoundException("Usuario", userId);
            }

            // Clear refresh token
            user.ClearRefreshToken();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User logged out successfully: {UserId}", userId);
        }

        public async Task ChangePasswordAsync(
            ChangePasswordRequestDto request,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation("Attempting password change for user: {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning(
                    "Password change failed: User not found: {UserId}",
                    request.UserId
                );
                throw new EntityNotFoundException("Usuario", request.UserId);
            }

            // Verify current password
            if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                _logger.LogWarning(
                    "Password change failed: Invalid current password for user: {UserId}",
                    request.UserId
                );
                throw new AuthenticationException("La contraseña actual es incorrecta");
            }

            // Hash and update new password
            var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            user.ChangePassword(newPasswordHash);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Password changed successfully for user: {UserId}",
                request.UserId
            );
        }

        public async Task<bool> ValidateUserRoleAsync(
            int userId,
            string requiredRole,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogDebug(
                "Validating role {RequiredRole} for user: {UserId}",
                requiredRole,
                userId
            );

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null || !user.IsActive)
            {
                _logger.LogWarning(
                    "Role validation failed: User not found or inactive: {UserId}",
                    userId
                );
                return false;
            }

            var hasRole = user.Role.Name.Equals(requiredRole, StringComparison.OrdinalIgnoreCase);

            _logger.LogDebug(
                "Role validation result for user {UserId}: {HasRole}",
                userId,
                hasRole
            );

            return hasRole;
        }

        public async Task<bool> CanAccessDepartmentAsync(
            int userId,
            int departmentId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogDebug(
                "Validating department access for user {UserId} to department {DepartmentId}",
                userId,
                departmentId
            );

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null || !user.IsActive)
            {
                _logger.LogWarning(
                    "Department access validation failed: User not found or inactive: {UserId}",
                    userId
                );
                return false;
            }

            // Director can access all departments (RF-INV-008)
            if (user.IsDirector)
            {
                _logger.LogDebug(
                    "Director access granted to all departments for user: {UserId}",
                    userId
                );
                return true;
            }

            // Administrator can access all departments for management purposes
            if (user.IsAdministrator)
            {
                _logger.LogDebug(
                    "Administrator access granted to all departments for user: {UserId}",
                    userId
                );
                return true;
            }

            // Section Manager can access their section's departments and view equipment in disuse from other areas (RF-INV-007)
            if (user.IsSectionManager)
            {
                // This would require checking if the department belongs to the user's section
                // For now, we allow access to their own department
                var canAccess = user.DepartmentId == departmentId;
                _logger.LogDebug(
                    "Section manager access result for user {UserId}: {CanAccess}",
                    userId,
                    canAccess
                );
                return canAccess;
            }

            // Other roles (Technician, EquipmentReceiver) can only access their own department
            var hasAccess = user.DepartmentId == departmentId;
            _logger.LogDebug(
                "Department access result for user {UserId}: {HasAccess}",
                userId,
                hasAccess
            );

            return hasAccess;
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(
            int userId,
            string refreshToken,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation("Attempting token refresh for user: {UserId}", userId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null || !user.IsActive)
            {
                _logger.LogWarning(
                    "Token refresh failed: User not found or inactive: {UserId}",
                    userId
                );
                throw new AuthenticationException("Usuario no encontrado o inactivo");
            }

            if (!user.HasValidRefreshToken() || user.RefreshToken != refreshToken)
            {
                _logger.LogWarning(
                    "Token refresh failed: Invalid or expired refresh token for user: {UserId}",
                    userId
                );
                throw new AuthenticationException("Token de actualización inválido o expirado");
            }

            // Generate new tokens
            var newToken = _jwtTokenGenerator.GenerateToken(user);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddMinutes(30);
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            // Update refresh token
            user.SetRefreshToken(newRefreshToken, refreshTokenExpiry);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Map to response DTO
            var response = _mapper.Map<LoginResponseDto>(user);

            _logger.LogInformation("Token refreshed successfully for user: {UserId}", userId);

            return response;
        }
    }    
}