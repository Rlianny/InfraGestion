namespace Application.DTOs.Personnel
{
    public class RateTechnicianRequest
    {
        public int? TechnicianId { get; set; }
        public string TechnicianName { get; set; } = string.Empty;
        public int? SuperiorId { get; set; }
        public string SuperiorUsername { get; set; } = string.Empty;

        public string Comments { get; set; } = string.Empty;
        public decimal Rate { get; set; }
    }
}
