using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public interface IReviewRoundService : IBaseService<ReviewRound, ReviewRoundDto>
{
    Task<List<ReviewRoundDto>> GetOpenRounds();
}
