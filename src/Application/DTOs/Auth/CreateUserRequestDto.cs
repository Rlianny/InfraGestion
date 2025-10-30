namespace Application.DTOs.Auth;

public record CreateUserRequestDto
{
    public required string FullName { get; init; }
    public required string Password { get; init; }
    public required string Role { get; init; }
    public required int DepartmentId { get; init; }
    public int? SectionId { get; init; }
    public int? YearsOfExperience { get; init; }
    public string? Specialty { get; init; }
}
