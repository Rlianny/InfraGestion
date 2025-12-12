using Domain.Enums;

namespace Application.DTOs.InspectionDTOs;

public class ReceivingInspectionRequestDto
{
    public int RequestId { get; set; }
    public DateTime RequestDate { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public int TechnicianId { get; set; }
    public string TechnicianFullName { get; set; }
    public InspectionRequestStatus Status { get; set; }
    public DecommissioningReason RejectReason { get; set; }


    public ReceivingInspectionRequestDto(int requestId, DateTime requestDate, int deviceId, string deviceName, int userId, string userFullName, int technicianId, string technicianFullName, InspectionRequestStatus status, DecommissioningReason rejectReason)
    {
        RequestId = requestId;
        RequestDate = requestDate;
        DeviceId = deviceId;
        DeviceName = deviceName;
        UserId = userId;
        UserFullName = userFullName;
        TechnicianId = technicianId;
        TechnicianFullName = technicianFullName;
        Status = status;
        RejectReason = rejectReason;
    }
}