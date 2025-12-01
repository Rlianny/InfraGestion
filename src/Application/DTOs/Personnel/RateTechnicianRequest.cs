namespace Application.DTOs.Personnel
{
    public class RateTechnicianRequest
    {
        public string TechnicianName { get; set; } = string.Empty;
        public string SuperiorUsername { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public double Rate { get; set; }
    }
}