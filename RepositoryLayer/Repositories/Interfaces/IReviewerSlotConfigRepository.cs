using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories.Interfaces;

public interface IReviewerSlotConfigRepository : IBaseRepository<ReviewerSlotConfig>
{
    Task<ReviewerSlotConfig?> GetByRound(int roundId);
}
