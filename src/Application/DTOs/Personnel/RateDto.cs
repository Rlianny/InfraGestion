namespace Application.DTOs.Personnel
{
    public class RateDto
    {
        public int RateId { get; set; }
        public int TechnicianId { get; set; }
        public int GiverId { get; set; }
        public string GiverName { get; set; } = string.Empty;
        public double Score { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}