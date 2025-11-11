using Domain.Enums;
public class PenaltyRequest
{
    public PenaltyType penaltyType { get; set; }
    public string Description { get; set; } = string.Empty;
    public int penalization { get; set; } = 0;
    public int TechnicianId { get; set; }
}