using Application.DTOs.Personnel;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Exceptions;

namespace Application.Services.Implementations
{
    public class PersonnelService : IPersonnelService
    {
        ITechnicianRepository technicianRepository { get; set; }
        IPerformanceRatingRepository performanceRatingRepository { get; set; }
        IUnitOfWork unitOfWork { get; set; }
        public PersonnelService(ITechnicianRepository technicianRepository, IPerformanceRatingRepository performanceRatingRepository,IUnitOfWork unitOfWork)
        {
            this.technicianRepository = technicianRepository;
            this.performanceRatingRepository = performanceRatingRepository;
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

        public async Task<TechnicianDto> GetTechnicianAsync(string technicianName)
        {
            var technician = await technicianRepository.GetByNameAsync(technicianName)
                ?? throw new EntityNotFoundException("Technician", technicianName);

            return new TechnicianDto
            {
                TechnicianId = technician.UserId,
                Name = technician.FullName,
                YearsOfExperience = technician.YearsOfExperience ?? 0,
                Specialty = technician.Specialty ?? string.Empty
            };
        }

        public async Task<List<BonusDto>> GetTechnicianBonusesAsync(string technicianName)
        {
            var technician = await technicianRepository.GetByNameAsync(technicianName)
                ?? throw new EntityNotFoundException("Technician", technicianName);
            var bonuses = (await performanceRatingRepository.GetRatingsByTechnicianAsync(technician.UserId)).Where(b => b.Score > 0);
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

        public async Task<List<PenaltyDto>> GetTechnicianPenaltyAsync(string technicianName)
        {
            var technician = await technicianRepository.GetByNameAsync(technicianName)
                ?? throw new EntityNotFoundException("Technician", technicianName);
            var penalties = (await performanceRatingRepository.GetRatingsByTechnicianAsync(technician.UserId)).Where(p => p.Score < 0);
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

        public async Task<IEnumerable<RateDto>> GetTechnicianPerformanceHistoryAsync(string technicianName)
        {
            var technician= await technicianRepository.GetByNameAsync(technicianName)
                ?? throw new EntityNotFoundException("Technician", technicianName);
            var ratings = await performanceRatingRepository.GetRatingsByTechnicianAsync(technician.UserId);
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
            await performanceRatingRepository.AddAsync(new Domain.Entities.PerformanceRating(
                 DateTime.Now,
                 request.Rate,
                 request.SuperiorId,
                 request.TechnicianId,
                 request.Comments
                 ));
                 await unitOfWork.SaveChangesAsync();
        }

        public async Task RegisterBonusAsync(BonusRequest request)
        {
            await performanceRatingRepository.AddAsync(new Domain.Entities.PerformanceRating(
               DateTime.Now,
               request.Bonus,
               request.SuperiorId,
               request.TechnicianId,
               request.Description
               ));
                await unitOfWork.SaveChangesAsync();
        }

        public async Task RegisterPenaltyAsync(PenaltyRequest request)
        {
            await performanceRatingRepository.AddAsync(new Domain.Entities.PerformanceRating(
               DateTime.Now,
               request.Penalization,
               request.SuperiorId,
               request.TechnicianId,
               request.Description
               ));
               await unitOfWork.SaveChangesAsync();
        }
    }
}