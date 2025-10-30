namespace Application.DTOs.Auth;

public record UpdateUserRequestDto
{
    public required int UserId { get; init; }

    public string? FullName { get; init; }

    public string? Role { get; init; }

    public int? DepartmentId { get; init; }

    public int? SectionId { get; init; }

    public bool? IsActive { get; init; }

    public int? YearsOfExperience { get; init; }

    public string? Specialty { get; init; }

    public decimal? Salary { get; init; }
}
