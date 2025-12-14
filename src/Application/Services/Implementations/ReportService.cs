using Application.DTOs.Report;
using Application.Services.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    internal class ReportService : IReportService
    {
        private readonly IPdfReportGenerator _pdfReportGenerator;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDecommissioningRequestRepository _decommissioningRepository;
        private readonly IMaintenanceRecordRepository _maintenanceRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IPerformanceRatingRepository _performanceRatingRepository;

        public ReportService(
            IPdfReportGenerator pdfReportGenerator,
            IDeviceRepository deviceRepository,
            ISectionRepository sectionRepository,
            IDecommissioningRequestRepository decommissioningRepository,
            IMaintenanceRecordRepository maintenanceRepository,
            IDepartmentRepository departmentRepository,
            ITransferRepository transferRepository,
            IUserRepository userRepository,
            IPerformanceRatingRepository performanceRatingRepository)
        {
            _pdfReportGenerator = pdfReportGenerator;
            _deviceRepository = deviceRepository;
            _sectionRepository = sectionRepository;
            _decommissioningRepository = decommissioningRepository;
            _maintenanceRepository = maintenanceRepository;
            _departmentRepository = departmentRepository;
            _transferRepository = transferRepository;
            _userRepository = userRepository;
            _performanceRatingRepository = performanceRatingRepository;
        }

        public async Task<IEnumerable<DeviceReportDto>> GenerateInventoryReportAsync(DeviceReportFilterDto filter)
        {
            var devices = await _deviceRepository.GetAllAsync();

            if (filter.DeviceType.HasValue)
                devices = devices.Where(d => d.Type == filter.DeviceType).ToList();

            if (filter.OperationalState.HasValue)
                devices = devices.Where(d => d.OperationalState == filter.OperationalState).ToList();

            if (!string.IsNullOrEmpty(filter.Department))
                devices = devices.Where(d => d.Name == filter.Department).ToList();

            var reportList = new List<DeviceReportDto>();

            foreach (var device in devices)
            {
                var maintenances = await _maintenanceRepository.GetMaintenancesByDeviceAsync(device.DeviceId);
                var department = await _departmentRepository.GetByIdAsync(device.DepartmentId);
                var section = await _sectionRepository.GetByIdAsync(department.SectionId);
                var maintenanceCount = maintenances.Count();
                var totalMaintenanceCost = maintenances.Sum(m => m.Cost);
                var lastMaintenanceDate = maintenances.OrderByDescending(m => m.Date).FirstOrDefault()?.Date;

                var reportDto = new DeviceReportDto(
                    device.DeviceId,
                    device.Name,
                    device.Type,
                    device.OperationalState,
                    device.DepartmentId,
                    device.Name,
                    department.SectionId,
                    section.Name,
                    device.AcquisitionDate,
                    maintenanceCount,
                    totalMaintenanceCost,
                    lastMaintenanceDate
                );

                reportList.Add(reportDto);
            }

            return reportList;
        }

        public async Task<IEnumerable<DecommissioningReportDto>> GenerateDischargeReportAsync(DecommissioningReportFilterDto filter)
        {
            var decommissioningRequests = (await _decommissioningRepository.GetDecommissioningRequestsByDateRangeAsync(filter.StartDate, filter.EndDate)).Where(d => d.IsApproved != null & (bool)d.IsApproved);

            var reportList = new List<DecommissioningReportDto>();

            foreach (var request in decommissioningRequests)
            {
                var device = await _deviceRepository.GetByIdAsync(request.DeviceId);
                var department = await _departmentRepository.GetByIdAsync((int)request.FinalDestinationDepartmentID);
                var receiver = await _userRepository.GetByIdAsync((int)request.DeviceReceiverId);
                var reportDto = new DecommissioningReportDto
                {
                    EquipmentId = request.DeviceId,
                    EquipmentName = device.Name,
                    DecommissionCause = request.Reason.ToString(),
                    FinalDestination = department.Name,
                    ReceiverName = receiver.FullName,
                    DecommissionDate = request.AnswerDate == null ? DateTime.MinValue : request.AnswerDate.Value
                };

                reportList.Add(reportDto);
            }

            return reportList;
        }

        public async Task<IEnumerable<PersonnelEffectivenessReportDto>> GeneratePersonnelEffectivenessReportAsync(PersonnelReportFilterDto criteria)
        {
            var users = await _userRepository.GetAllAsync();
            var reportList = new List<PersonnelEffectivenessReportDto>();

            foreach (var user in users)
            {
                var reportDto = new PersonnelEffectivenessReportDto();
                reportList.Add(reportDto);
            }

            return reportList;
        }

        public async Task<IEnumerable<DeviceReplacementReportDto>> GenerateEquipmentReplacementReportAsync()
        {
            var devices = await _deviceRepository.GetAllAsync();
            var reportList = new List<DeviceReplacementReportDto>();

            foreach (var device in devices)
            {
                var reportDto = new DeviceReplacementReportDto(device.DeviceId, device.Name);

                reportList.Add(reportDto);
            }

            return reportList;
        }

        public async Task<IEnumerable<DepartmentTransferReportDto>> GenerateDepartmentTransferReportAsync(string departmentId)
        {
            if (!int.TryParse(departmentId, out int deptId))
                return new List<DepartmentTransferReportDto>();
            var department = await _departmentRepository.GetByIdAsync(deptId);
            var transfers = await _transferRepository.GetAllAsync();
            var departmentTransfers = transfers.Where(t =>
                t.SourceSectionId == department.SectionId ||
                t.DestinationSectionId == department.SectionId
            ).ToList();

            var reportList = new List<DepartmentTransferReportDto>();

            foreach (var transfer in departmentTransfers)
            {
                var reportDto = new DepartmentTransferReportDto
                {
                };

                reportList.Add(reportDto);
            }

            return reportList;
        }

        public async Task<IEnumerable<CorrelationAnalysisReportDto>> GenerateCorrelationAnalysisReportAsync()
        {
            var reportList = new List<CorrelationAnalysisReportDto>();
            return reportList;
        }

        public async Task<IEnumerable<BonusDeterminationReportDto>> GenerateBonusDeterminationReportAsync(BonusReportCriteria criteria)
        {
            var reportList = new List<BonusDeterminationReportDto>();
            return reportList;
        }

        public async Task<PdfExportDto> GeneratePdfReport(string reportType)
        {
            switch (reportType)
            {
                case "correlation-analysis":
                    var table = await GenerateCorrelationAnalysisReportAsync();
                    Table table1 = null;
                    var pdf = await _pdfReportGenerator.CreatePdfTable(table1);
                    return new PdfExportDto(pdf);
                default:
                    throw new Exception("Invalid report type");
            }
        }

        private string DetermineReplacementStatus(Domain.Entities.Device device, IEnumerable<Domain.Aggregations.MaintenanceRecord> maintenances)
        {
            var yearsSinceAcquisition = (DateTime.Now - device.AcquisitionDate).TotalDays / 365;
            var maintenanceCount = maintenances.Count();

            if (yearsSinceAcquisition > 5 || maintenanceCount > 10)
                return "Critical";
            else if (yearsSinceAcquisition > 3 || maintenanceCount > 5)
                return "Warning";
            else
                return "Good";
        }
    }
}
