using Application.DTOs.Report;
using Application.Services.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Interfaces;
using System.Text;

namespace Application.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IPdfReportGenerator _pdfReportGenerator;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDecommissioningRequestRepository _decommissioningRepository;
        private readonly IMaintenanceRecordRepository _maintenanceRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITechnicianRepository _technicianRepository;
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
            ITechnicianRepository technicianRepository,
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
            _technicianRepository = technicianRepository;
            _performanceRatingRepository = performanceRatingRepository;
        }

        

        public async Task<Report<DecommissioningReportDto>> GenerateDischargeReportAsync()
        {
            var decommissioningRequests = (await _decommissioningRepository.GetAllAsync())
                .Where(
                        d => d.Status == RequestStatus.Approved
                        && d.AnswerDate >= DateTime.Now.AddYears(-1)
                );


            var reportList = new List<DecommissioningReportDto>();

            foreach (var request in decommissioningRequests)
            {
                var device = await _deviceRepository.GetByIdAsync(request.DeviceId);
                var department = await _departmentRepository.GetByIdAsync((int)request.FinalDestinationDepartmentID!);
                var receiver = await _userRepository.GetByIdAsync((int)request.DeviceReceiverId!);

                var reportDto = new DecommissioningReportDto
                {
                    EquipmentId = request.DeviceId,
                    EquipmentName = device?.Name ?? string.Empty,
                    DecommissionCause = request.Reason.ToString(),
                    FinalDestination = department?.Name ?? string.Empty,
                    ReceiverName = receiver?.FullName ?? string.Empty,
                };

                reportList.Add(reportDto);
            }

            return await Report(reportList);
        }

        public async Task<Report<PersonnelEffectivenessReportDto>> GeneratePersonnelEffectivenessReportAsync(PersonnelReportFilterDto criteria)
        {
            var users = await _userRepository.GetAllAsync();
            var maintenanceRecords = await _maintenanceRepository.GetAllAsync();
            var decommissioningRequests = await _decommissioningRepository.GetAllAsync();
            var performanceRatings = await _performanceRatingRepository.GetAllAsync();

            var filteredUsers = users.AsEnumerable();

            if (criteria.DepartmentId.HasValue)
                filteredUsers = filteredUsers.Where(u => u.DepartmentId == criteria.DepartmentId);

            if (criteria.SectionId.HasValue)
                filteredUsers = filteredUsers.Where(u => u.Department != null && u.Department.SectionId == criteria.SectionId);

            if (!string.IsNullOrEmpty(criteria.Specialty))
                filteredUsers = filteredUsers.Where(u => u.Specialty == criteria.Specialty);

            if (criteria.MinimumYearsOfExperience.HasValue)
                filteredUsers = filteredUsers.Where(u => (u.YearsOfExperience ?? 0) >= criteria.MinimumYearsOfExperience);

            if (criteria.MaximumYearsOfExperience.HasValue)
                filteredUsers = filteredUsers.Where(u => (u.YearsOfExperience ?? 0) <= criteria.MaximumYearsOfExperience);

            var reportList = new List<PersonnelEffectivenessReportDto>();

            foreach (var user in filteredUsers)
            {
                var userMaintenances = maintenanceRecords.Where(m => m.TechnicianId == user.UserId).ToList();
                var userDecommissionings = decommissioningRequests.Where(d => d.TechnicianId == user.UserId).ToList();
                var userRatings = performanceRatings.Where(r => r.TechnicianId == user.UserId).ToList();

                var totalInterventions = userMaintenances.Count() + userDecommissionings.Count();
                var totalMaintenanceCost = userMaintenances.Sum(m => m.Cost);
                var averageCostPerIntervention = totalInterventions > 0 ? totalMaintenanceCost / totalInterventions : 0;
                var averageRating = userRatings.Any() ? userRatings.Average(r => r.Score) : 0;
                var lastInterventionDate = userMaintenances
                    .OrderByDescending(m => m.Date)
                    .FirstOrDefault()?.Date;

                var department = user.Department;
                var section = department?.Section;

                var reportDto = new PersonnelEffectivenessReportDto
                {
                    TechnicianId = user.UserId,
                    TechnicianName = user.FullName,
                    Specialty = user.Specialty ?? string.Empty,
                    YearsOfExperience = user.YearsOfExperience ?? 0,
                    MaintenanceInterventions = userMaintenances.Count,
                    DecommissioningRequests = userDecommissionings.Count,
                    TotalInterventions = totalInterventions,
                    TotalMaintenanceCost = totalMaintenanceCost,
                    AverageCostPerIntervention = averageCostPerIntervention,
                    AverageRating = averageRating,
                    LastInterventionDate = lastInterventionDate,
                    DepartmentName = department?.Name ?? string.Empty,
                    SectionName = section?.Name ?? string.Empty
                };

                if (criteria.MinimumAverageRating.HasValue && averageRating < criteria.MinimumAverageRating)
                    continue;

                if (criteria.OnlyActiveInPeriod.HasValue && criteria.OnlyActiveInPeriod.Value)
                {
                    var fromDate = criteria.FromDate ?? DateTime.MinValue;
                    var toDate = criteria.ToDate ?? DateTime.MaxValue;

                    var hasActivity = userMaintenances.Any(m => m.Date >= fromDate && m.Date <= toDate) ||
                                     userDecommissionings.Any(d => d.EmissionDate >= fromDate && d.EmissionDate <= toDate);

                    if (!hasActivity)
                        continue;
                }

                reportList.Add(reportDto);
            }

            return await Report(reportList);
        }

        public async Task<Report<DeviceReplacementReportDto>> GenerateEquipmentReplacementReportAsync()
        {
            var devices = await _deviceRepository.GetAllAsync();
            var reportList = new List<DeviceReplacementReportDto>();

            var oneYearAgo = DateTime.Now.AddYears(-1);

            foreach (var device in devices)
            {
                var maintenances = await _maintenanceRepository.GetMaintenancesByDeviceAsync(device.DeviceId);
                var recentMaintenances = maintenances.Where(m => m.Date >= oneYearAgo).ToList();

                if (recentMaintenances.Count > 3)
                {

                    var totalMaintenanceCost = maintenances.Sum(m => m.Cost);

                    var lastMaintenanceDate = maintenances.OrderByDescending(m => m.Date).FirstOrDefault()?.Date;

                    var reportDto = new DeviceReplacementReportDto()
                    {
                        DeviceId = device.DeviceId,
                        DeviceName = device.Name,
                        MaintenanceCountLastYear = recentMaintenances.Count,
                        TotalMaintenanceCost = totalMaintenanceCost,
                    };

                    reportList.Add(reportDto);
                }
            }

            return await Report(reportList);
        }

        public async Task<Report<SectionTransferReportDto>> GenerateTransferReportAsync()
        {


            var transfers = await _transferRepository.GetAllAsync();


            var reportList = new List<SectionTransferReportDto>();
            foreach (var transfer in transfers)
            {
                var device = await _deviceRepository.GetByIdAsync(transfer.DeviceId);
                var sourceSection = await _sectionRepository.GetByIdAsync(transfer.SourceSectionId);

                var destinationSection = await _sectionRepository.GetByIdAsync(transfer.DestinationSectionId);

                var receiver = await _userRepository.GetByIdAsync(transfer.DeviceReceiverId);

                var reportDto = new SectionTransferReportDto
                {
                    TransferId = transfer.TransferId,
                    DeviceName = device?.Name ?? string.Empty,
                    TransferDate = transfer.Date,
                    SourceSectionName = sourceSection?.Name ?? string.Empty,
                    DestinationSectionName = destinationSection?.Name ?? string.Empty,
                    ReceiverName = receiver?.FullName ?? string.Empty,
                };

                reportList.Add(reportDto);
            }

            return await Report(reportList);
        }

        public async Task<Report<CorrelationAnalysisReportDto>> GenerateCorrelationAnalysisReportAsync()
        {
            
            var maintenances = await _maintenanceRepository.GetAllAsync();
            var decommissioningRequests = await _decommissioningRepository.GetAllAsync();
            var performanceRatings = await _performanceRatingRepository.GetAllAsync();

            var reportList = new List<CorrelationAnalysisReportDto>();
            var technicians = await _technicianRepository.GetAllAsync();
            foreach (var technician in technicians)
            {
                var tecnichianMaintenances = await _maintenanceRepository.GetAllAsync();
                var technicianDecommissionings = 
                    (await _decommissioningRepository.GetDecommissioningRequestsByTechnicianAsync(technician.UserId))
                    .Where(d => d.Status==RequestStatus.Approved);
                //Equpos con danno irreparable
                var irreparableFailures =
                    technicianDecommissionings
                    .Where(
                     d => d.Reason == DecommissioningReason.IrreparableTechnicalFailure);
                    

                //Calcular costo de cada equipo
                var costByDevice = irreparableFailures.Select(

                    d =>
                    maintenances.Sum(
                        m => m.DeviceId != d.DeviceId
                        ? 0 : m.Cost
                    )

                    );


                //Calcular longevidad de cada equipo
                var longevityByDevice = new List<double>();
                foreach (var decomissoning in irreparableFailures)
                {
                    var device = await _deviceRepository.GetByIdAsync(decomissoning.DeviceId);
                    var decommisioningDate = decomissoning.AnswerDate??DateTime.Now;
                    var arrivingDate = device!.AcquisitionDate;
                    longevityByDevice.Add(
                        Convert.ToDouble(decommisioningDate.Subtract(arrivingDate).Days)
                        );
                }

                var correlationIndex = CorrelationQuotient(costByDevice.ToArray(),longevityByDevice.ToArray());

                var technicianRatings = await _performanceRatingRepository.GetRatingsByTechnicianAsync(technician.UserId);
                var averageRating = technicianRatings.Any() ? technicianRatings.Average(r => r.Score) : 0;
                var reportDto = new CorrelationAnalysisReportDto
                {
                    TechnicianName = technician.FullName,
                    Specialty = technician.Specialty ?? string.Empty,
                    CorrelationIndex = correlationIndex,
                    AveragePerformanceRating = averageRating,
                };

                reportList.Add(reportDto);
            }

            return await Report(reportList
                .OrderBy(r => r.CorrelationIndex)
                .Take(5)
                );
        }

        public async Task<Report<BonusDeterminationReportDto>> GenerateBonusDeterminationReportAsync()
        {
            var maintenances = await _maintenanceRepository.GetAllAsync();
            var decommissioningRequests = await _decommissioningRepository.GetAllAsync();
            var performanceRatings = await _performanceRatingRepository.GetAllAsync();
            var technicians = await _technicianRepository.GetAllAsync();

            var reportList = new List<BonusDeterminationReportDto>();

            foreach (var technician in technicians)
            {
                var technicianMaintenances = await _maintenanceRepository.GetMaintenancesByTechnicianAsync(technician.UserId);
                var technicianDecommissionings = (await _decommissioningRepository.GetDecommissioningRequestsByTechnicianAsync(technician.UserId))
                    .Where(d => d.Status == RequestStatus.Approved);


                var totalInterventions = technicianMaintenances.Count() + technicianDecommissionings.Count();


                var technicianRatings = await (_performanceRatingRepository.GetRatingsByTechnicianAsync(technician.UserId));


                var averageRating = technicianRatings.FirstOrDefault() is null ? 0 : technicianRatings.Average(r => r.Score);
                var highestRating = technicianRatings.FirstOrDefault() is null ? 0 : technicianRatings.Max(r => r.Score);
                var lowestRating = technicianRatings.FirstOrDefault() is null ? 0 : technicianRatings.Min(r => r.Score);


                var reportDto = new BonusDeterminationReportDto
                {
                    TechnicianName = technician.FullName,
                    TotalInterventions = totalInterventions,
                    AverageRating = averageRating,
                    HighestRating = highestRating,
                    LowestRating = lowestRating,
                    RatingCount = technicianRatings.Count(),
                };

                reportList.Add(reportDto);
            }
            return await Report(reportList);
        }
        public async Task<Report<DeviceMantainenceReportDto>> GenerateDeviceMantainanceReportAsync(int deviceId)
        {
            var mantainances = await _maintenanceRepository.GetMaintenancesByDeviceAsync(deviceId);
            var response = new List<DeviceMantainenceReportDto>();
            foreach (var maintenance in mantainances)
            {
                var techichian = await _technicianRepository.GetByIdAsync(maintenance.TechnicianId) ?? throw new EntityNotFoundException("Techichian", maintenance.TechnicianId);
                response.Add(
                new DeviceMantainenceReportDto
                {
                    Date = maintenance.Date,
                    Description = maintenance.Description,
                    Type = maintenance.Type == MaintenanceType.Preventive ? "Preventivo" :
                    maintenance.Type == MaintenanceType.Corrective ? "Correctivo" :
                    "Predictivo",
                    Technician = techichian.FullName
                });
            }
            return await Report(response);
        }

        private async Task<Report<T>> Report<T>(IEnumerable<T> reportData)
        {
            var pdfBytes = await _pdfReportGenerator.GeneratePdf(reportData);
            return new Report<T>() { ReportData = reportData, PdfBytes = pdfBytes };
        }
        private static double SS(double[] X, double[] Y)
        {

            int n = X.Length;
            if (n==0)
            {
                return 0;
            }
            double SS_XY = 0;
            for (int i = 0; i < n; i++)
            {
                SS_XY += X[i] * Y[i];
            }
            SS_XY -= (X.Sum() + Y.Sum()) / n;
            return SS_XY;
        }
        private static double CorrelationQuotient(double[] X,double[] Y)
        {
            double SS_XX = SS(X, X);
            double SS_YY = SS(Y, Y);
            if (SS_XX*SS_YY<double.Epsilon)
            {
                return 0;
            }
            return SS(X,Y)/Math.Sqrt(SS(X,X)*SS(Y,Y));
        }

    }
}
