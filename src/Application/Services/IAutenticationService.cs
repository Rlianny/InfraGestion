public interface IAuthenticationService
{
    Task LoginAsync(string identifier, string password);
    Task LogoutAsync(string userId);
    Task UpdateUserAsync(string userId);
    Task DeactivateUserAsync(string userId);
    Task RequestPasswordResetAsync(string email);
    Task ResetPasswordAsync(string token, string newPassword);
}