using Domain.Enums;

namespace Application.DTOs.DevicesDTOs
{
    public class RegisterDeviceDto
    {
        public string Name { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public int TechnicianId { get; set; } // Id del técnico responsable de la inspeccion inicial del dispositivo
        public int UserId { get; set; } // Id del usuario que regisra  el dispositivo en el sistema
    }
}