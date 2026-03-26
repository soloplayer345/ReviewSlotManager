using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;
using AutoMapper;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public class ReviewRoundService : BaseService<ReviewRound, ReviewRoundDto>, IReviewRoundService
{
    private readonly IReviewRoundRepository _roundRepository;
    private readonly IMapper _mapper;

    public ReviewRoundService(IReviewRoundRepository roundRepository, IMapper mapper)
        : base(roundRepository, mapper)
    {
        _roundRepository = roundRepository;
        _mapper = mapper;
    }

    public async Task<List<ReviewRoundDto>> GetOpenRounds()
    {
        var rounds = await _roundRepository.GetOpenRounds();
        return _mapper.Map<List<ReviewRoundDto>>(rounds);
    }

    public async Task<ReviewRoundDto> Create(CreateReviewRoundDto dto)
    {
        var entity = new ReviewRound
        {
            SemesterId = dto.SemesterId,
            RoundNumber = dto.RoundNumber,
            RoundName = dto.RoundName,
            RegistrationOpenAt = dto.RegistrationOpenAt,
            RegistrationCloseAt = dto.RegistrationCloseAt,
            ReviewDateFrom = dto.ReviewDateFrom,
            ReviewDateTo = dto.ReviewDateTo,
            Status = ReviewRoundStatus.Upcoming
        };

        var created = await _roundRepository.Create(entity);
        return _mapper.Map<ReviewRoundDto>(created);
    }

    public async Task<ReviewRoundDto> Update(int id, UpdateReviewRoundDto dto)
    {
        var existing = await _roundRepository.Read(id)
            ?? throw new KeyNotFoundException($"ReviewRound {id} not found.");

        if (!Enum.TryParse<ReviewRoundStatus>(dto.Status, true, out var status))
            throw new ArgumentException($"Invalid status '{dto.Status}'.");

        existing.RoundName = dto.RoundName;
        existing.RegistrationOpenAt = dto.RegistrationOpenAt;
        existing.RegistrationCloseAt = dto.RegistrationCloseAt;
        existing.ReviewDateFrom = dto.ReviewDateFrom;
        existing.ReviewDateTo = dto.ReviewDateTo;
        existing.Status = status;

        await _roundRepository.Update(existing);
        return _mapper.Map<ReviewRoundDto>(existing);
    }
}
