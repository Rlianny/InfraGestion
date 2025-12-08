using Domain.Enums;

namespace Application.DTOs.Personnel
{
    public class BonusRequest
    {
        public BonusType BonusType { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Bonus { get; set; } = 0;
        public string TechnicianName { get; set; } = string.Empty;
        public string SuperiorUsername { get; set; } = string.Empty;
    }
}