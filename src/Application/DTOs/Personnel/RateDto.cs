namespace Application.DTOs.Personnel
{
    public class RateDto
    {
        public int GiverId { get; set; }
        public double Rate { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}