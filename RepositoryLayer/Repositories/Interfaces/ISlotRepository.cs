using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories.Interfaces;

public interface ISlotRepository : IBaseRepository<Slot>
{
    Task<List<Slot>> GetAvailableSlotsByRound(int roundId);
}
