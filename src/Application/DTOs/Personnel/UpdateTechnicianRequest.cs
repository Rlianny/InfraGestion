namespace Application.DTOs.Personnel
{
    public class UpdateTechnicianRequest
    {
        public int TechnicianId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Specialty { get; set; }
        public int? YearsOfExperience { get; set; }
        public int? DepartmentId { get; set; }
    }
}
