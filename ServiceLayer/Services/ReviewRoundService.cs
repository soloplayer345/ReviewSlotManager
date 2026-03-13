using AutoMapper;
using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public class ReviewRoundService : BaseService<ReviewRound, ReviewRoundDto>
{
    private readonly ReviewRoundRepository _roundRepository;
    private readonly IMapper _mapper;

    public ReviewRoundService(ReviewRoundRepository roundRepository, IMapper mapper)
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
}
