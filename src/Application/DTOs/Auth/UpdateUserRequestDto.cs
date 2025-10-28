namespace Application.DTOs.Auth
{
    public class UpdateUserRequestDto
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public int? DepartmentId { get; set; }
        public int? SectionId { get; set; }
        public bool? IsActive { get; set; }

        // Fields specific to Technician
        public int? YearsOfExperience { get; set; }
        public string? Specialty { get; set; }
        public decimal? Salary { get; set; }
    }
}