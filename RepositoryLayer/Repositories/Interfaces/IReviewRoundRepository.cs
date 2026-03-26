using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories.Interfaces;

public interface IReviewRoundRepository : IBaseRepository<ReviewRound>
{
    Task<List<ReviewRound>> GetOpenRounds();
}
