namespace Application.DTOs.Auth;

public record LoginResponseDto
{
    public required int UserId { get; init; }
    public required string FullName { get; init; }
    public required string Role { get; init; }
    public required int DepartmentId { get; init; }
    public required string DepartmentName { get; init; }
    public required string Token { get; init; }
    public required DateTime ExpiresAt { get; init; }
}
