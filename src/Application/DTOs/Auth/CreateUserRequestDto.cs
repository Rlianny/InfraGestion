namespace Application.DTOs.Auth
{
    public class CreateUserRequestDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Administrator, Director, etc.
        public int DepartmentId { get; set; }
        public int? SectionId { get; set; } // for SectionManager

        // Fields specific to Technician
        public int? YearsOfExperience { get; set; }
        public string? Specialty { get; set; }
        public decimal? Salary { get; set; }
    }
}