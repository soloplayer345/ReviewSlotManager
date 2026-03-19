using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories;

public interface ISlotRepository : IBaseRepository<Slot>
{
    Task<List<Slot>> GetAvailableSlotsByRound(int roundId);
}
