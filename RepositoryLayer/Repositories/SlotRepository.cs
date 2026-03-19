using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;

namespace RepositoryLayer.Repositories;

public class SlotRepository : BaseRepository<Slot>, ISlotRepository
{
    private readonly ReviewSlotDbContext _context;

    public SlotRepository(ReviewSlotDbContext context)
        : base(context, x => x.SlotId)
    {
        _context = context;
    }

    public Task<List<Slot>> GetAvailableSlotsByRound(int roundId)
    {
        return _context.Slots
            .AsNoTracking()
            .Where(x => x.RoundId == roundId && (x.Status == SlotStatus.Open || x.Status == SlotStatus.Full))
            .OrderBy(x => x.StartTime)
            .ToListAsync();
    }
}
