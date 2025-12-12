using Application.DTOs.Maintenance;
using Application.DTOs.Transfer;
using Application.DTOs.Decommissioning;
using Domain.Enums;

namespace Application.DTOs.DevicesDTOs
{
    public class DeviceDetailDto : DeviceDto
    {
        public DeviceDetailDto(int deviceId, string name, DeviceType deviceType, OperationalState operationalState, string departmentName, IEnumerable<MaintenanceRecordDto> maintenanceHistory, IEnumerable<TransferDto> transferHistory, DecommissioningDto? decommissioningInfo, DateTime date) : base(deviceId, name, deviceType, operationalState, departmentName)
        {
            MaintenanceHistory = maintenanceHistory;
            TransferHistory = transferHistory;
            DecommissioningInfo = decommissioningInfo;
            AcquisitionDate = date;
        }
        public DateTime AcquisitionDate { get; set; }
        public IEnumerable<MaintenanceRecordDto> MaintenanceHistory { get; set; }
        public IEnumerable<TransferDto> TransferHistory { get; set; }
        public DecommissioningDto? DecommissioningInfo { get; set; }
    }
}