using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces;

public interface IReviewRoundService : IBaseService<ReviewRound, ReviewRoundDto>
{
    Task<List<ReviewRoundDto>> GetOpenRounds();
    Task<List<ReviewRoundDto>> GetBySemester(int semesterId);
    Task<ReviewRoundDto> Create(CreateReviewRoundDto dto);
    Task<ReviewRoundDto> Update(int id, UpdateReviewRoundDto dto);
}
