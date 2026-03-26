using RepositoryLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories;

public class ReviewerSlotConfigRepository : BaseRepository<ReviewerSlotConfig>, IReviewerSlotConfigRepository
{
    private readonly ReviewSlotDbContext _context;

    public ReviewerSlotConfigRepository(ReviewSlotDbContext context)
        : base(context, x => x.ConfigId)
    {
        _context = context;
    }

    public Task<ReviewerSlotConfig?> GetByRound(int roundId)
    {
        return _context.ReviewerSlotConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RoundId == roundId);
    }
}
