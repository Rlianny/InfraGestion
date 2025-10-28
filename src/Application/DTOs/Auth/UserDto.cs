namespace Application.DTOs.Auth
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int? SectionId { get; set; }
        public string? SectionName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Fields specific to Technician (nullables)
        public int? YearsOfExperience { get; set; }
        public string? Specialty { get; set; }
        public decimal? Salary { get; set; }
        public decimal? CurrentPerformanceRating { get; set; }
    }
}