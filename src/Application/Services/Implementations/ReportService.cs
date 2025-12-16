using Application.DTOs.Report;
using Application.Services.Interfaces;
using Domain.Aggregations;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                devices = devices.Where(d => d.Type == filter.DeviceType);

            if (filter.OperationalState.HasValue)
                devices = devices.Where(d => d.OperationalState == filter.OperationalState);

            if (filter.DepartmentId.HasValue)
                devices = devices.Where(d => d.DepartmentId == filter.DepartmentId);

            if (filter.FromDate.HasValue)
                devices = devices.Where(d => d.AcquisitionDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                devices = devices.Where(d => d.AcquisitionDate <= filter.ToDate.Value);

            var reportList = new List<DeviceReportDto>();

            foreach (var device in devices)
            {
                var maintenances = await _maintenanceRepository.GetMaintenancesByDeviceAsync(device.DeviceId);
                var department = await _departmentRepository.GetByIdAsync(device.DepartmentId) ?? throw new EntityNotFoundException("Department", device.DepartmentId);
                var section = await _sectionRepository.GetByIdAsync(department.SectionId) ?? throw new EntityNotFoundException("Section", department.SectionId);

                var maintenanceCount = maintenances.Count();
                var totalMaintenanceCost = maintenances.Sum(m => m.Cost);
                var lastMaintenanceDate = maintenances.OrderByDescending(m => m.Date).FirstOrDefault()?.Date;

                var reportDto = new DeviceReportDto(
                    device.DeviceId,
                    device.Name,
                    device.Type,
                    device.OperationalState,
                    device.DepartmentId,
                    department.Name,
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
            var startDate = filter.FromDate ?? DateTime.MinValue;
            var endDate = filter.ToDate ?? DateTime.MaxValue;

            var decommissioningRequests = await _decommissioningRepository.GetDecommissioningRequestsByDateRangeAsync(startDate, endDate);

            var query = decommissioningRequests
                .Where(d => d.IsApproved != null && d.IsApproved == true);

            if (filter.Reason.HasValue)
                query = query.Where(d => d.Reason == filter.Reason);

            if (filter.Status.HasValue)
                query = query.Where(d => d.Status == filter.Status);

            var reportList = new List<DecommissioningReportDto>();

            foreach (var request in query)
            {
                var device = await _deviceRepository.GetByIdAsync(request.DeviceId);
                var department = await _departmentRepository.GetByIdAsync((int)request.FinalDestinationDepartmentID);
                var receiver = await _userRepository.GetByIdAsync((int)request.DeviceReceiverId);

                var reportDto = new DecommissioningReportDto
                {
                    EquipmentId = request.DeviceId,
                    EquipmentName = device?.Name ?? string.Empty,
                    DecommissionCause = request.Reason.ToString(),
                    FinalDestination = department?.Name ?? string.Empty,
                    ReceiverName = receiver?.FullName ?? string.Empty,
                    DecommissionDate = request.AnswerDate ?? DateTime.MinValue
                };

                reportList.Add(reportDto);
            }

            return reportList;
        }

        public async Task<IEnumerable<PersonnelEffectivenessReportDto>> GeneratePersonnelEffectivenessReportAsync(PersonnelReportFilterDto criteria)
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

            return reportList;
        }

        public async Task<IEnumerable<DeviceReplacementReportDto>> GenerateEquipmentReplacementReportAsync()
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

            return reportList;
        }

        public async Task<IEnumerable<SectionTransferReportDto>> GenerateTransferReportAsync()
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

            return reportList;
        }

        public async Task<IEnumerable<CorrelationAnalysisReportDto>> GenerateCorrelationAnalysisReportAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var maintenances = await _maintenanceRepository.GetAllAsync();
            var decommissioningRequests = await _decommissioningRepository.GetAllAsync();
            var performanceRatings = await _performanceRatingRepository.GetAllAsync();

            var reportList = new List<CorrelationAnalysisReportDto>();
            var technicians = users.Where(u => u.Role.RoleId == (int)RoleEnum.Technician).ToList();

            int rank = 1;

            foreach (var technician in technicians)
            {
                var technicianMaintenances = maintenances.Where(m => m.TechnicianId == technician.UserId).ToList();
                var technicianDecommissionings = decommissioningRequests
                    .Where(d => d.TechnicianId == technician.UserId && d.IsApproved == true).ToList();

                var irreparableFailures = technicianDecommissionings
                    .Where(d => d.Reason == DecommissioningReason.IrreparableTechnicalFailure)
                    .Count();

                var uniqueEquipment = new HashSet<int>(
                    technicianMaintenances.Select(m => m.DeviceId)
                        .Union(technicianDecommissionings.Select(d => d.DeviceId))
                ).Count;

                var totalMaintenanceCost = technicianMaintenances.Sum(m => m.Cost);
                var averageCostPerEquipment = uniqueEquipment > 0 ? totalMaintenanceCost / uniqueEquipment : 0;

                var technicianRatings = performanceRatings
                    .Where(r => r.TechnicianId == technician.UserId)
                    .ToList();

                var averageRating = technicianRatings.Any() ? technicianRatings.Average(r => r.Score) : 0;

                var averageLongevity = 0.0;
                if (technicianDecommissionings.Count != 0)
                {
                    var averageLongevitySum = 0.0;
                    foreach (var decom in technicianDecommissionings)
                    {
                        var device = await _deviceRepository.GetByIdAsync(decom.DeviceId);
                        if (device != null)
                        {
                            var longevity = (decom.EmissionDate - device.AcquisitionDate).TotalDays / 365;
                            averageLongevitySum += longevity;
                        }
                    }
                    averageLongevity = technicianDecommissionings.Count > 0
                        ? averageLongevitySum / technicianDecommissionings.Count
                        : 0;
                }

                var correlationIndex = (averageCostPerEquipment / 1000) * (10 / Math.Max(averageLongevity, 0.1));
                var totalBonuses = CalculateBonuses(averageRating, uniqueEquipment);
                var totalPenalties = CalculatePenalties(averageRating, technicianDecommissionings.Count);
                var netBalance = totalBonuses - totalPenalties;

                var reportDto = new CorrelationAnalysisReportDto
                {
                    Rank = rank++,
                    TechnicianId = technician.UserId,
                    TechnicianName = technician.FullName,
                    Specialty = technician.Specialty ?? string.Empty,
                    YearsOfExperience = technician.YearsOfExperience ?? 0,
                    EquipmentType = "Mixed",
                    EquipmentCount = uniqueEquipment,
                    DecommissionedEquipmentCount = technicianDecommissionings.Count,
                    IrreparableFailureCount = irreparableFailures,
                    TotalMaintenanceCost = totalMaintenanceCost,
                    AverageMaintenanceCostPerEquipment = averageCostPerEquipment,
                    AverageEquipmentLongevity = averageLongevity,
                    CorrelationIndex = correlationIndex,
                    AveragePerformanceRating = averageRating,
                    TotalBonuses = totalBonuses,
                    TotalPenalties = totalPenalties,
                    NetBalance = netBalance,
                    Observations = GenerateCorrelationObservations(correlationIndex, irreparableFailures, averageRating)
                };

                reportList.Add(reportDto);
            }

            return reportList
                .OrderByDescending(r => r.CorrelationIndex)
                .Take(5)
                .ToList();
        }

        public async Task<IEnumerable<BonusDeterminationReportDto>> GenerateBonusDeterminationReportAsync(BonusReportCriteria criteria)
        {
            var users = await _userRepository.GetAllAsync();
            var maintenances = await _maintenanceRepository.GetAllAsync();
            var decommissioningRequests = await _decommissioningRepository.GetAllAsync();
            var performanceRatings = await _performanceRatingRepository.GetAllAsync();

            var filteredUsers = users.Where(u => u.RoleId ==(int)RoleEnum.Technician).AsEnumerable();

            if (criteria.DepartmentId.HasValue)
                filteredUsers = filteredUsers.Where(u => u.DepartmentId == criteria.DepartmentId);

            if (criteria.SectionId.HasValue)
                filteredUsers = filteredUsers.Where(u => u.Department != null && u.Department.SectionId == criteria.SectionId);

            if (criteria.OnlyActiveTechnicians.HasValue && criteria.OnlyActiveTechnicians.Value)
                filteredUsers = filteredUsers.Where(u => u.IsActive);

            var reportList = new List<BonusDeterminationReportDto>();

            foreach (var technician in filteredUsers)
            {
                var technicianMaintenances = maintenances.Where(m => m.TechnicianId == technician.UserId).ToList();
                var technicianDecommissionings = decommissioningRequests
                    .Where(d => d.TechnicianId == technician.UserId && d.IsApproved == true)
                    .ToList();

                var totalInterventions = technicianMaintenances.Count + technicianDecommissionings.Count;

                if (criteria.MinimumInterventions.HasValue && totalInterventions < criteria.MinimumInterventions)
                    continue;

                var technicianRatings = performanceRatings
                    .Where(r => r.TechnicianId == technician.UserId)
                    .ToList();

                var averageRating = technicianRatings.Count != 0 ? technicianRatings.Average(r => r.Score) : 0;
                var highestRating = technicianRatings.Count != 0 ? technicianRatings.Max(r => r.Score) : 0;
                var lowestRating = technicianRatings.Count != 0 ? technicianRatings.Min(r => r.Score) : 0;

                var totalMaintenanceCost = technicianMaintenances.Sum(m => m.Cost);

                var totalBonuses = CalculateBonuses(averageRating, totalInterventions);
                var totalPenalties = CalculatePenalties(averageRating, technicianDecommissionings.Count);

                var effectivenessIndex = CalculateEffectivenessIndex(averageRating, totalInterventions, totalMaintenanceCost);

                var netBalance = totalBonuses - totalPenalties;
                var recommendation = GenerateBonusRecommendation(effectivenessIndex, averageRating);

                var reportDto = new BonusDeterminationReportDto
                {
                    TechnicianId = technician.UserId,
                    TechnicianName = technician.FullName,
                    Specialty = technician.Specialty ?? string.Empty,
                    YearsOfExperience = technician.YearsOfExperience ?? 0,
                    DepartmentName = technician.Department?.Name ?? string.Empty,
                    TotalInterventions = totalInterventions,
                    MaintenanceCount = technicianMaintenances.Count,
                    DecommissioningCount = technicianDecommissionings.Count,
                    TotalMaintenanceCost = totalMaintenanceCost,
                    AverageRating = averageRating,
                    HighestRating = highestRating,
                    LowestRating = lowestRating,
                    RatingCount = technicianRatings.Count,
                    TotalBonuses = totalBonuses,
                    TotalPenalties = totalPenalties,
                    EffectivenessIndex = effectivenessIndex,
                    Recommendation = recommendation,
                    SalaryAdjustmentAmount = netBalance,
                    AdjustmentType = netBalance > 0 ? "Bonificación" : (netBalance < 0 ? "Penalización" : "Sin ajuste"),
                    Comments = GenerateBonusComments(technician, averageRating, effectivenessIndex)
                };

                reportList.Add(reportDto);
            }

            if (!string.IsNullOrEmpty(criteria.SortBy))
            {
                reportList = SortBonusReport(reportList, criteria.SortBy);
            }

            return reportList;
        }

        public async Task<PdfExportDto> GeneratePdfReport(string reportType)
        {
            switch (reportType.ToLower())
            {
                case "inventory":
                    {
                        var data = await GenerateInventoryReportAsync(new DeviceReportFilterDto());
                        return new PdfExportDto( await _pdfReportGenerator.GeneratePdf(data));
                    }
                case "decommissioning":
                    {
                        var data = await GenerateDischargeReportAsync(new DecommissioningReportFilterDto());
                        return new PdfExportDto(await _pdfReportGenerator.GeneratePdf(data));
                    }
                case "personnel-effectiveness":
                    {
                        var data = await GeneratePersonnelEffectivenessReportAsync(new PersonnelReportFilterDto());
                        return new PdfExportDto(await _pdfReportGenerator.GeneratePdf(data));
                    }
                case "equipment-replacement":
                    {
                        var data = await GenerateEquipmentReplacementReportAsync();
                        return new PdfExportDto(await _pdfReportGenerator.GeneratePdf(data));
                    }
                case "correlation-analysis":
                    {
                        var data = await GenerateCorrelationAnalysisReportAsync();
                        return new PdfExportDto(await _pdfReportGenerator.GeneratePdf(data));
                    }
                case "bonus-determination":
                    {
                        var data = await GenerateBonusDeterminationReportAsync(new BonusReportCriteria());
                        return new PdfExportDto(await _pdfReportGenerator.GeneratePdf(data));
                    }
                case "transfers":
                    {
                        var data = await GenerateTransferReportAsync();
                        return new PdfExportDto(await _pdfReportGenerator.GeneratePdf(data));
                    }
                default:
                    throw new ArgumentException($"Tipo de reporte inválido: {reportType}");
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

        private string GenerateCorrelationObservations(double correlationIndex, int irreparableFailures, double averageRating)
        {
            var observations = new StringBuilder();

            if (correlationIndex > 5)
                observations.Append("Índice de correlación muy alto. ");

            if (irreparableFailures >= 3)
                observations.Append("Múltiples equipos dados de baja por fallo irreparable. ");

            if (averageRating < 3)
                observations.Append("Calificación promedio baja. ");

            if (observations.Length == 0)
                observations.Append("Rendimiento dentro de parámetros esperados.");

            return observations.ToString();
        }

        private double CalculateBonuses(double averageRating, int totalInterventions)
        {
            double bonus = 0;

            if (averageRating >= 4.5)
                bonus += 500;
            else if (averageRating >= 4.0)
                bonus += 300;

            bonus += totalInterventions * 10;

            return bonus;
        }

        private double CalculatePenalties(double averageRating, int decommissioningCount)
        {
            double penalty = 0;

            if (averageRating <= 2.5)
                penalty += 200;
            else if (averageRating <= 3.0)
                penalty += 100;

            penalty += decommissioningCount * 50;

            return penalty;
        }

        private double CalculateEffectivenessIndex(double averageRating, int totalInterventions, double totalMaintenanceCost)
        {
            if (totalMaintenanceCost == 0)
                return averageRating * totalInterventions;

            return (averageRating * totalInterventions) / (totalMaintenanceCost / 1000);
        }

        private string GenerateBonusRecommendation(double effectivenessIndex, double averageRating)
        {
            if (effectivenessIndex > 10 && averageRating >= 4)
                return "Otorgar bonificación máxima";
            else if (effectivenessIndex > 5 && averageRating >= 3.5)
                return "Otorgar bonificación moderada";
            else if (effectivenessIndex > 2 && averageRating >= 3)
                return "Otorgar bonificación mínima";
            else if (averageRating < 2.5)
                return "Aplicar penalización";
            else
                return "Sin cambios de salario";
        }

        private string GenerateBonusComments(Domain.Entities.User technician, double averageRating, double effectivenessIndex)
        {
            var comments = new StringBuilder();

            comments.Append($"Técnico con {technician.YearsOfExperience ?? 0} años de experiencia. ");
            comments.Append($"Calificación promedio: {averageRating:F2}. ");
            comments.Append($"Índice de efectividad: {effectivenessIndex:F2}. ");

            if (averageRating >= 4)
                comments.Append("Desempeño excelente. ");
            else if (averageRating >= 3)
                comments.Append("Desempeño satisfactorio. ");
            else
                comments.Append("Desempeño requiere mejora. ");

            return comments.ToString();
        }

        private List<BonusDeterminationReportDto> SortBonusReport(List<BonusDeterminationReportDto> report, string sortBy)
        {
            return sortBy.ToLower() switch
            {
                "rating" => report.OrderByDescending(r => r.AverageRating).ToList(),
                "effectiveness" => report.OrderByDescending(r => r.EffectivenessIndex).ToList(),
                "interventions" => report.OrderByDescending(r => r.TotalInterventions).ToList(),
                "bonus" => report.OrderByDescending(r => r.SalaryAdjustmentAmount).ToList(),
                _ => report
            };
        }
    }
}
