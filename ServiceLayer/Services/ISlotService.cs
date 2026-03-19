using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public interface ISlotService : IBaseService<Slot, SlotDto>
{
    Task<List<SlotDto>> GetAvailableSlotsByRound(int roundId);
}
