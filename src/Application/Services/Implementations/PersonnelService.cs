using Application.DTOs.Personnel;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Exceptions;
using Application.DTOs.Inventory;
using Domain.Enums;
using Domain.Aggregations;
using Domain.Entities;

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
        private readonly IUnitOfWork unitOfWork;

        public PersonnelService(
            ITechnicianRepository technicianRepository,
            IPerformanceRatingRepository performanceRatingRepository,
            IUserRepository userRepository,
            IDeviceRepository devicesRepository,
            IReceivingInspectionRequestRepository receivingInspectionRequestRepository,
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork)
        {
            this.technicianRepository = technicianRepository;
            this.performanceRatingRepository = performanceRatingRepository;
            this.userRepository = userRepository;
            this.devicesRepository = devicesRepository;
            this.receivingInspectionRequestRepository = receivingInspectionRequestRepository;
            this.unitOfWork = unitOfWork;
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
                    Specialty = technician.Specialty ?? string.Empty
                };
                technicianDtos.Add(dto);
            }
            return technicianDtos;
        }

        public async Task<TechnicianDto> GetTechnicianAsync(int technicianId)
        {
            var technician = await technicianRepository.GetByIdAsync(technicianId)
                ?? throw new EntityNotFoundException("Technician", technicianId);

            return new TechnicianDto
            {
                TechnicianId = technician.UserId,
                Name = technician.FullName,
                YearsOfExperience = technician.YearsOfExperience ?? 0,
                Specialty = technician.Specialty ?? string.Empty
            };
        }

        public async Task<TechnicianDto> UpdateTechnicianAsync(UpdateTechnicianRequest request)
        {
            var technician = await technicianRepository.GetByIdAsync(request.TechnicianId)
                ?? throw new EntityNotFoundException("Technician", request.TechnicianId);

            // Actualizar perfil básico
            technician.UpdateProfile(request.FullName, request.Specialty);

            // Actualizar años de experiencia si se proporciona
            if (request.YearsOfExperience.HasValue && request.Specialty != null)
            {
                technician.SetTechnicalExperience(request.YearsOfExperience.Value, request.Specialty);
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
                Specialty = technician.Specialty ?? string.Empty
            };
        }
        public async Task<IEnumerable<DeviceDto>> GetTechnicianPendingDevicesAsync(int technicianId)
        {
            var receivingInspections = await receivingInspectionRequestRepository.GetReceivingInspectionRequestsByTechnicianAsync(technicianId);
            var deviceDtos = new List<DeviceDto>();
            foreach (var inspection in receivingInspections)
            {
                var device = await devicesRepository.GetByIdAsync(inspection.DeviceId);
                if (device != null && device.OperationalState == OperationalState.UnderRevision)
                {
                    var dto = new DeviceDto
                    (
                     device.DeviceId, device.Name, device.Type, device.OperationalState, (await departmentRepository.GetByIdAsync(device.DepartmentId))?.Name ?? "Unassigned"
                    );
                    deviceDtos.Add(dto);
                }
            }
            return deviceDtos;
        }

        public async Task<List<BonusDto>> GetTechnicianBonusesAsync(int technicianId)
        {
            var technician = await technicianRepository.GetByIdAsync(technicianId)
                ?? throw new EntityNotFoundException("Technician", technicianId);
            var bonuses = (await performanceRatingRepository.GetRatingsByTechnicianAsync(technicianId)).Where(b => b.Score > 0);
            var bonusDtos = new List<BonusDto>();
            foreach (var bonus in bonuses)
            {
                var dto = new BonusDto
                {
                    Bonus = bonus.Score,
                    Description = bonus.Description
                };
                bonusDtos.Add(dto);
            }
            return bonusDtos;
        }

        public async Task<List<PenaltyDto>> GetTechnicianPenaltyAsync(int technicianId)
        {
            var technician = await technicianRepository.GetByIdAsync(technicianId)
                ?? throw new EntityNotFoundException("Technician", technicianId);
            var penalties = (await performanceRatingRepository.GetRatingsByTechnicianAsync(technicianId)).Where(p => p.Score < 0);
            var penaltiesDtos = new List<PenaltyDto>();
            foreach (var penalty in penalties)
            {
                var dto = new PenaltyDto
                {
                    Penalty = Math.Abs(penalty.Score),
                    Description = penalty.Description
                };
                penaltiesDtos.Add(dto);
            }
            return penaltiesDtos;
        }

        public async Task<IEnumerable<RateDto>> GetTechnicianPerformanceHistoryAsync(int technicianId)
        {
            var technician = await technicianRepository.GetByIdAsync(technicianId)
                ?? throw new EntityNotFoundException("Technician", technicianId);
            var ratings = await performanceRatingRepository.GetRatingsByTechnicianAsync(technicianId);
            var ratingDtos = new List<RateDto>();
            foreach (var rating in ratings)
            {
                var dto = new RateDto
                {
                    GiverId = rating.UserId,
                    Rate = rating.Score,
                    Comment = rating.Description
                };
                ratingDtos.Add(dto);
            }
            return ratingDtos;
        }

        public async Task RateTechnicianPerformanceAsync(RateTechnicianRequest request)
        {
            var technician = await technicianRepository.GetByNameAsync(request.TechnicianName)
                ?? throw new EntityNotFoundException("Technician", request.TechnicianName);
            var superior = await userRepository.GetByUsernameAsync(request.SuperiorUsername)
                ?? throw new EntityNotFoundException("User", request.SuperiorUsername);

            await performanceRatingRepository.AddAsync(new Domain.Entities.PerformanceRating(
                DateTime.Now,
                request.Rate,
                superior.UserId,
                technician.UserId,
                request.Comments
            ));
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RegisterBonusAsync(BonusRequest request)
        {
            var technician = await technicianRepository.GetByNameAsync(request.TechnicianName)
                ?? throw new EntityNotFoundException("Technician", request.TechnicianName);
            var superior = await userRepository.GetByUsernameAsync(request.SuperiorUsername)
                ?? throw new EntityNotFoundException("User", request.SuperiorUsername);

            await performanceRatingRepository.AddAsync(new Domain.Entities.PerformanceRating(
                DateTime.Now,
                request.Bonus,
                superior.UserId,
                technician.UserId,
                request.Description
            ));
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RegisterPenaltyAsync(PenaltyRequest request)
        {
            var technician = await technicianRepository.GetByNameAsync(request.TechnicianName)
                ?? throw new EntityNotFoundException("Technician", request.TechnicianName);
            var superior = await userRepository.GetByUsernameAsync(request.SuperiorUsername)
                ?? throw new EntityNotFoundException("User", request.SuperiorUsername);

            await performanceRatingRepository.AddAsync(new Domain.Entities.PerformanceRating(
                DateTime.Now,
                -Math.Abs(request.Penalization),
                superior.UserId,
                technician.UserId,
                request.Description
            ));
            await unitOfWork.SaveChangesAsync();
        }
    }
}