using Application.DTOs.Decommissioning;
using Application.DTOs.Inventory;
using Application.DTOs.Maintenance;
using Application.DTOs.Personnel;
using Application.Services.Interfaces;
using Domain.Aggregations;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services.Implementations
{
    public class PersonnelService : IPersonnelService
    {
        private readonly ITechnicianRepository technicianRepository;
        private readonly IPerformanceRatingRepository performanceRatingRepository;
        private readonly IUserRepository userRepository;
        private readonly IDeviceRepository devicesRepository;
        private readonly IReceivingInspectionRequestRepository receivingInspectionRequestRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMaintenanceRecordRepository maintenanceRecordRepository;
        private readonly IDecommissioningRequestRepository decommissioningRequestRepository;
        private readonly IUnitOfWork unitOfWork;

        public PersonnelService(
            ITechnicianRepository technicianRepository,
            IPerformanceRatingRepository performanceRatingRepository,
            IUserRepository userRepository,
            IDeviceRepository devicesRepository,
            IReceivingInspectionRequestRepository receivingInspectionRequestRepository,
            IDepartmentRepository departmentRepository,
            IMaintenanceRecordRepository maintenanceRecordRepository,
            IDecommissioningRequestRepository decommissioningRequestRepository,
            IUnitOfWork unitOfWork
        )
        {
            this.technicianRepository = technicianRepository;
            this.performanceRatingRepository = performanceRatingRepository;
            this.userRepository = userRepository;
            this.devicesRepository = devicesRepository;
            this.receivingInspectionRequestRepository = receivingInspectionRequestRepository;
            this.departmentRepository = departmentRepository;
            this.maintenanceRecordRepository = maintenanceRecordRepository;
            this.decommissioningRequestRepository = decommissioningRequestRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<TechnicianDetailDto> GetTechnicianDetailAsync(int technicianId)
        {
            var technician =
                await technicianRepository.GetByIdAsync(technicianId)
                ?? throw new EntityNotFoundException("Technician", technicianId);

            var maintenanceRecords =
                await maintenanceRecordRepository.GetMaintenancesByTechnicianAsync(technicianId);
            var decommissioningRequests =
                await decommissioningRequestRepository.GetDecommissioningRequestsByTechnicianAsync(
                    technicianId
                );

            // Preload devices and receivers to enrich DTOs with names
            var deviceIds = maintenanceRecords
                .Select(m => m.DeviceId)
                .Concat(decommissioningRequests.Select(d => d.DeviceId))
                .Distinct()
                .ToList();

            var deviceLookup = new Dictionary<int, Domain.Entities.Device?>();
            foreach (var deviceId in deviceIds)
            {
                var device = await devicesRepository.GetByIdAsync(deviceId);
                deviceLookup[deviceId] = device;
            }

            var receiverIds = decommissioningRequests
                .Select(d => d.DeviceReceiverId)
                .Distinct()
                .ToList();
            var receiverLookup = new Dictionary<int, User?>();
            foreach (var receiverId in receiverIds)
            {
                var receiver = await userRepository.GetByIdAsync(receiverId);
                receiverLookup[receiverId] = receiver;
            }

            var maintenanceRecordDtos = new List<MaintenanceRecordDto>();
            foreach (var record in maintenanceRecords)
            {
                deviceLookup.TryGetValue(record.DeviceId, out var device);
                var dto = new MaintenanceRecordDto
                {
                    MaintenanceRecordId = record.MaintenanceRecordId,
                    DeviceId = record.DeviceId,
                    DeviceName = device?.Name ?? $"Equipo {record.DeviceId}",
                    Description = record.Description,
                    MaintenanceDate = record.Date,
                    TechnicianId = technicianId,
                    TechnicianName = technician.FullName,
                    Cost = record.Cost,
                    MaintenanceType = record.Type,
                };
                maintenanceRecordDtos.Add(dto);
            }

            var decommissioningRequestDtos = new List<DecommissioningRequestDto>();
            foreach (var request in decommissioningRequests)
            {
                deviceLookup.TryGetValue(request.DeviceId, out var device);
                receiverLookup.TryGetValue(request.DeviceReceiverId, out var receiver);
                var dto = new DecommissioningRequestDto
                {
                    DecommissioningRequestId = request.DecommissioningRequestId,
                    DeviceId = request.DeviceId,
                    DeviceName = device?.Name ?? $"Equipo {request.DeviceId}",
                    Reason = request.Reason,
                    RequestDate = request.Date,
                    TechnicianId = technicianId,
                    TechnicianName = technician.FullName,
                    DeviceReceiverId = request.DeviceReceiverId,
                    DeviceReceiverName =
                        receiver?.FullName ?? $"Usuario {request.DeviceReceiverId}",
                    // IsApproved = request.IsApproved,
                    Status = MapToDecommissioningStatus(request.Status),
                };
                decommissioningRequestDtos.Add(dto);
            }

            return new TechnicianDetailDto
            {
                TechnicianId = technician.UserId,
                Name = technician.FullName,
                YearsOfExperience = technician.YearsOfExperience ?? 0,
                Specialty = technician.Specialty ?? string.Empty,
                MaintenanceRecords = maintenanceRecordDtos,
                DecommissioningRequests = decommissioningRequestDtos,
            };
        }

        public async Task<IEnumerable<TechnicianDto>> GetAllTechniciansAsync()
        {
            var technicians = await technicianRepository.GetAllAsync();
            var technicianDtos = new List<TechnicianDto>();
            foreach (var technician in technicians)
            {
                var dto = new TechnicianDto
                {
                    TechnicianId = technician.UserId,
                    Name = technician.FullName,
                    YearsOfExperience = technician.YearsOfExperience ?? 0,
                    Specialty = technician.Specialty ?? string.Empty,
                };
                technicianDtos.Add(dto);
            }
            return technicianDtos;
        }

        private static DecommissioningStatus MapToDecommissioningStatus(RequestStatus status)
        {
            return status switch
            {
                RequestStatus.Pending => DecommissioningStatus.Pending,
                RequestStatus.Approved => DecommissioningStatus.Accepted,
                RequestStatus.Rejected => DecommissioningStatus.Rejected,
                _ => DecommissioningStatus.Pending,
            };
        }

        public async Task<TechnicianDto> GetTechnicianAsync(int technicianId)
        {
            var technician =
                await technicianRepository.GetByIdAsync(technicianId)
                ?? throw new EntityNotFoundException("Technician", technicianId);

            return new TechnicianDto
            {
                TechnicianId = technician.UserId,
                Name = technician.FullName,
                YearsOfExperience = technician.YearsOfExperience ?? 0,
                Specialty = technician.Specialty ?? string.Empty,
            };
        }

        public async Task<TechnicianDto> UpdateTechnicianAsync(UpdateTechnicianRequest request)
        {
            var technician =
                await technicianRepository.GetByIdAsync(request.TechnicianId)
                ?? throw new EntityNotFoundException("Technician", request.TechnicianId);

            // Actualizar perfil básico
            technician.UpdateProfile(request.FullName, request.Specialty);

            // Actualizar años de experiencia si se proporciona
            if (request.YearsOfExperience.HasValue && request.Specialty != null)
            {
                technician.SetTechnicalExperience(
                    request.YearsOfExperience.Value,
                    request.Specialty
                );
            }

            // Cambiar departamento si se proporciona
            if (request.DepartmentId.HasValue)
            {
                technician.ChangeDepartment(request.DepartmentId.Value);
            }

            await unitOfWork.SaveChangesAsync();

            return new TechnicianDto
            {
                TechnicianId = technician.UserId,
                Name = technician.FullName,
                YearsOfExperience = technician.YearsOfExperience ?? 0,
                Specialty = technician.Specialty ?? string.Empty,
            };
        }

        public async Task<IEnumerable<DeviceDto>> GetTechnicianPendingDevicesAsync(int technicianId)
        {
            var receivingInspections =
                await receivingInspectionRequestRepository.GetReceivingInspectionRequestsByTechnicianAsync(
                    technicianId
                );
            var deviceDtos = new List<DeviceDto>();
            foreach (var inspection in receivingInspections)
            {
                var device = await devicesRepository.GetByIdAsync(inspection.DeviceId);
                if (device != null && device.OperationalState == OperationalState.UnderRevision)
                {
                    var dto = new DeviceDto(
                        device.DeviceId,
                        device.Name,
                        device.Type,
                        device.OperationalState,
                        (await departmentRepository.GetByIdAsync(device.DepartmentId))?.Name
                            ?? "Unassigned"
                    );
                    deviceDtos.Add(dto);
                }
            }
            return deviceDtos;
        }

        public async Task<List<BonusDto>> GetTechnicianBonusesAsync(int technicianId)
        {
            var technician =
                await technicianRepository.GetByIdAsync(technicianId)
                ?? throw new EntityNotFoundException("Technician", technicianId);
            var bonuses = (
                await performanceRatingRepository.GetRatingsByTechnicianAsync(technicianId)
            )
                .Where(b => b.Score > 0)
                .ToList();

            return bonuses
                .Select(bonus => new BonusDto
                {
                    BonusId = bonus.PerformanceRatingId,
                    TechnicianId = technicianId,
                    Amount = bonus.Score,
                    Description = bonus.Description,
                    Date = bonus.Date,
                })
                .ToList();
        }

        public async Task<List<PenaltyDto>> GetTechnicianPenaltyAsync(int technicianId)
        {
            var technician =
                await technicianRepository.GetByIdAsync(technicianId)
                ?? throw new EntityNotFoundException("Technician", technicianId);
            var penalties = (
                await performanceRatingRepository.GetRatingsByTechnicianAsync(technicianId)
            )
                .Where(p => p.Score < 0)
                .ToList();

            return penalties
                .Select(penalty => new PenaltyDto
                {
                    PenaltyId = penalty.PerformanceRatingId,
                    TechnicianId = technicianId,
                    Amount = Math.Abs(penalty.Score),
                    Description = penalty.Description,
                    Date = penalty.Date,
                })
                .ToList();
        }

        public async Task<IEnumerable<RateDto>> GetTechnicianPerformanceHistoryAsync(
            int technicianId
        )
        {
            var technician =
                await technicianRepository.GetByIdAsync(technicianId)
                ?? throw new EntityNotFoundException("Technician", technicianId);
            var ratings = await performanceRatingRepository.GetRatingsByTechnicianAsync(
                technicianId
            );

            // Preload givers to avoid n+1
            var giverIds = ratings.Select(r => r.UserId).Distinct().ToList();
            var giverLookup = new Dictionary<int, User?>();
            foreach (var giverId in giverIds)
            {
                giverLookup[giverId] = await userRepository.GetByIdAsync(giverId);
            }

            return ratings
                .Select(rating =>
                {
                    giverLookup.TryGetValue(rating.UserId, out var giver);
                    return new RateDto
                    {
                        RateId = rating.PerformanceRatingId,
                        TechnicianId = technicianId,
                        GiverId = rating.UserId,
                        GiverName = giver?.FullName ?? $"Usuario {rating.UserId}",
                        Score = rating.Score,
                        Comment = rating.Description,
                        Date = rating.Date,
                    };
                })
                .ToList();
        }

        public async Task RateTechnicianPerformanceAsync(RateTechnicianRequest request)
        {
            var technician = await ResolveTechnicianAsync(
                request.TechnicianId,
                request.TechnicianName
            );
            var superior = await ResolveSuperiorAsync(request.SuperiorId, request.SuperiorUsername);

            await AddPerformanceRatingAsync(
                (double)request.Rate,
                superior,
                technician,
                request.Comments
            );
        }

        public async Task RegisterBonusAsync(BonusRequest request)
        {
            var technician = await ResolveTechnicianAsync(
                request.TechnicianId,
                request.TechnicianName
            );
            var superior = await ResolveSuperiorAsync(request.SuperiorId, request.SuperiorUsername);

            await AddPerformanceRatingAsync(
                (double)request.Amount,
                superior,
                technician,
                request.Description
            );
        }

        public async Task RegisterPenaltyAsync(PenaltyRequest request)
        {
            var technician = await ResolveTechnicianAsync(
                request.TechnicianId,
                request.TechnicianName
            );
            var superior = await ResolveSuperiorAsync(request.SuperiorId, request.SuperiorUsername);

            await AddPerformanceRatingAsync(
                -Math.Abs((double)request.Amount),
                superior,
                technician,
                request.Description
            );
        }

        private async Task<User> ResolveTechnicianAsync(int? technicianId, string? technicianName)
        {
            if (technicianId.HasValue && technicianId.Value > 0)
            {
                return await technicianRepository.GetByIdAsync(technicianId.Value)
                    ?? throw new EntityNotFoundException("Technician", technicianId.Value);
            }

            if (!string.IsNullOrWhiteSpace(technicianName))
            {
                return await technicianRepository.GetByNameAsync(technicianName)
                    ?? throw new EntityNotFoundException("Technician", technicianName);
            }

            throw new ArgumentException("Se requiere TechnicianId o TechnicianName");
        }

        private async Task<User> ResolveSuperiorAsync(int? superiorId, string? superiorUsername)
        {
            if (superiorId.HasValue && superiorId.Value > 0)
            {
                return await userRepository.GetByIdAsync(superiorId.Value)
                    ?? throw new EntityNotFoundException("User", superiorId.Value);
            }

            if (!string.IsNullOrWhiteSpace(superiorUsername))
            {
                return await userRepository.GetByUsernameAsync(superiorUsername)
                    ?? throw new EntityNotFoundException("User", superiorUsername);
            }

            throw new ArgumentException("Se requiere SuperiorId o SuperiorUsername");
        }

        private async Task AddPerformanceRatingAsync(
            double score,
            User superior,
            User technician,
            string description
        )
        {
            await performanceRatingRepository.AddAsync(
                new PerformanceRating(
                    DateTime.Now,
                    score,
                    superior.UserId,
                    technician.UserId,
                    description
                )
            );

            await unitOfWork.SaveChangesAsync();
        }
    }
}
