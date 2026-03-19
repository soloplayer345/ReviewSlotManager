using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;

namespace RepositoryLayer.Repositories;

public class ReviewRoundRepository : BaseRepository<ReviewRound>, IReviewRoundRepository
{
    private readonly ReviewSlotDbContext _context;

    public ReviewRoundRepository(ReviewSlotDbContext context)
        : base(context, x => x.RoundId)
    {
        _context = context;
    }

    public Task<List<ReviewRound>> GetOpenRounds()
    {
        var now = DateTime.UtcNow;
        return _context.ReviewRounds
            .AsNoTracking()
            .Where(x => x.Status == ReviewRoundStatus.Open && now >= x.RegistrationOpenAt && now <= x.RegistrationCloseAt)
            .OrderBy(x => x.RoundNumber)
            .ToListAsync();
    }
}
