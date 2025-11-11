namespace Application.DTOs.Auth;

public record DeactivateUserRequestDto
{
    public required int UserId { get; init; }
    public required string Reason { get; init; }

}
