using Domain.Enums;

namespace Application.DTOs.Personnel
{
    public class BonusRequest
    {
        public BonusType BonusType { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Bonus { get; set; } = 0;
        public int TechnicianId { get; set; }
        public int SuperiorId { get; set; }
    }
}