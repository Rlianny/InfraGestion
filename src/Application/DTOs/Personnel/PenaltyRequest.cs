using Domain.Enums;

namespace Application.DTOs.Personnel
{
    public class PenaltyRequest
    {
        public string Description { get; set; } = string.Empty;
        public double Penalization { get; set; } = 0;
        public string SuperiorUsername { get; set; } = string.Empty;
        public string TechnicianName { get; set; } = string.Empty;
    }
}