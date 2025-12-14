using Application.DTOs.Decommissioning;
using Application.DTOs.Maintenance;
using Application.DTOs.Transfer;
using Domain.Enums;

namespace Application.DTOs.DevicesDTOs
{
    public class DeviceDetailDto : DeviceDto
    {
        public DeviceDetailDto(
            int deviceId,
            string name,
            DeviceType deviceType,
            OperationalState operationalState,
            string departmentName,
            IEnumerable<MaintenanceRecordDto> maintenanceHistory,
            IEnumerable<TransferDto> transferHistory,
            IEnumerable<DecommissioningRequestDto> decommissioningRequestsInfo,
            DateTime date,
            string sectionName,
            string? sectionManagerName
        )
            : base(deviceId, name, deviceType, operationalState, departmentName)
        {
            MaintenanceHistory = maintenanceHistory;
            TransferHistory = transferHistory;
            DecommissioningRequestInfo = decommissioningRequestsInfo;
            AcquisitionDate = date;
            SectionName = sectionName;
            SectionManagerName = sectionManagerName;
        }

        public DateTime AcquisitionDate { get; set; }
        public IEnumerable<MaintenanceRecordDto> MaintenanceHistory { get; set; }
        public IEnumerable<TransferDto> TransferHistory { get; set; }
        public IEnumerable<DecommissioningRequestDto> DecommissioningRequestInfo { get; set; }
        public string SectionName { get; set; }
        public string? SectionManagerName { get; set; }
    }
}
