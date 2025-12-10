namespace Application.DTOs.Personnel
{
    public class BonusDto
    {
        public int BonusId { get; set; }
        public int TechnicianId { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}