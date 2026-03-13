using RepositoryLayer.Data;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;

namespace RepositoryLayer.Repositories;

public class ReviewRoundRepository : BaseRepository<ReviewRound>
{
    private readonly InMemoryDataStore _store;

    public ReviewRoundRepository(InMemoryDataStore store)
        : base(store.ReviewRounds, x => x.RoundId, (x, id) => x.RoundId = id)
    {
        _store = store;
    }

    public Task<List<ReviewRound>> GetOpenRounds()
    {
        var now = DateTime.UtcNow;
        var rounds = _store.ReviewRounds
            .Where(x => x.Status == ReviewRoundStatus.Open && now >= x.RegistrationOpenAt && now <= x.RegistrationCloseAt)
            .OrderBy(x => x.RoundNumber)
            .ToList();

        return Task.FromResult(rounds);
    }
}
