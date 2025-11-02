namespace Application.DTOs.Auth;

public record ChangePasswordRequestDto
{
    public required int UserId { get; init; }
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }
    public required string ConfirmNewPassword { get; init; }
}
