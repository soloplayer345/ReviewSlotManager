using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces;

public interface IReviewerSlotConfigService : IBaseService<ReviewerSlotConfig, ReviewerSlotConfigDto>
{
    Task<ReviewerSlotConfigDto?> GetByRound(int roundId);
    Task<ReviewerSlotConfigDto> Create(CreateReviewerSlotConfigDto dto);
    Task<ReviewerSlotConfigDto> Update(int id, UpdateReviewerSlotConfigDto dto);
}
