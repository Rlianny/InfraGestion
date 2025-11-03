using Application.DTOs.Maintenance;
using Application.DTOs.Transfer;
using Application.DTOs.Decommissioning;
namespace Application.DTOs.Inventory
{
    public class DeviceDetailDto : DeviceDto
    {
        public List<MaintenanceRecordSummaryDto> MaintenanceHistory { get; set; } = new();
        public List<TransferSummaryDto> TransferHistory { get; set; } = new();
        public DecommissioningDto? DecommissioningInfo { get; set; }
    }
}