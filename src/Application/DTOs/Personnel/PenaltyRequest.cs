namespace Application.DTOs.Personnel
{
    public class PenaltyRequest
    {
        public int? TechnicianId { get; set; }

        public string TechnicianName { get; set; } = string.Empty;

        public int? SuperiorId { get; set; }

        public string SuperiorUsername { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;
    }
}