using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories;

public interface IReviewRoundRepository : IBaseRepository<ReviewRound>
{
    Task<List<ReviewRound>> GetOpenRounds();
}
