namespace Application.DTOs.Personnel
{
    public class RateTechnicianRequest
    {
        public int TechnicianId { get; set; }
        public int SuperiorId { get; set; }
        public string Comments { get; set; } = string.Empty;
        public double Rate { get; set; }
    }
}