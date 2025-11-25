using Domain.Enums;
public class BonusRequest
{
    public BonusType BonusType { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Bonus { get; set; } = 0;
    public int TechnicianId { get; set; }
    public int SuperiorId { get; set; }
}