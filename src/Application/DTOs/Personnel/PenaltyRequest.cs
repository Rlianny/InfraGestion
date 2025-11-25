using Domain.Enums;
public class PenaltyRequest
{
    public string Description { get; set; } = string.Empty;
    public int penalization { get; set; } = 0;
    public int SuperiorId { get; set; }
    public int TechnicianId { get; set; }
}