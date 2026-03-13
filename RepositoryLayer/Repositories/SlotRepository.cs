using RepositoryLayer.Data;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;

namespace RepositoryLayer.Repositories;

public class SlotRepository : BaseRepository<Slot>
{
    private readonly InMemoryDataStore _store;

    public SlotRepository(InMemoryDataStore store)
        : base(store.Slots, x => x.SlotId, (x, id) => x.SlotId = id)
    {
        _store = store;
    }

    public Task<List<Slot>> GetAvailableSlotsByRound(int roundId)
    {
        var slots = _store.Slots
            .Where(x => x.RoundId == roundId && x.Status is SlotStatus.Open or SlotStatus.Full)
            .OrderBy(x => x.StartTime)
            .ToList();

        return Task.FromResult(slots);
    }
}
