using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces;

public interface ISlotService : IBaseService<Slot, SlotDto>
{
    Task<List<SlotDto>> GetAvailableSlotsByRound(int roundId);
    Task<List<SlotDetailsDto>> GetDetailsByRound(int roundId);
    Task<SlotDto> Create(CreateSlotDto dto);
    Task<SlotDto> Update(int id, UpdateSlotDto dto);
}
