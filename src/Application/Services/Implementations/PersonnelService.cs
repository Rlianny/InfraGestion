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
    public PersonnelService(ITechnicianRepository technicianRepository, IPerformanceRatingRepository performanceRatingRepository)
    {
        this.technicianRepository = technicianRepository;
        this.performanceRatingRepository = performanceRatingRepository;
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
                Name = technician.Username,
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
            Name = technician.Username,
            YearsOfExperience = technician.YearsOfExperience ?? 0,
            Specialty = technician.Specialty ?? string.Empty
        };
    }

    public async Task<List<BonusDto>> GetTechnicianBonusesAsync(int technicianId)
    {
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
        await performanceRatingRepository.AddAsync(new Domain.Entities.PerformanceRating(
             DateTime.Now,
             request.Rate,
             request.SuperiorId,
             request.TechnicianId,
             request.Comments
             ));
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
    }

    public Task RegisterPenaltyAsync(PenaltyRequest request)
    {
        return performanceRatingRepository.AddAsync(new Domain.Entities.PerformanceRating(
           DateTime.Now,
           request.Penalization,
           request.SuperiorId,
           request.TechnicianId,
           request.Description
           ));
    }
}
}