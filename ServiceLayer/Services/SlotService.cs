using AutoMapper;
using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public class SlotService : BaseService<Slot, SlotDto>
{
    private readonly SlotRepository _slotRepository;
    private readonly IMapper _mapper;

    public SlotService(SlotRepository slotRepository, IMapper mapper)
        : base(slotRepository, mapper)
    {
        _slotRepository = slotRepository;
        _mapper = mapper;
    }

    public async Task<List<SlotDto>> GetAvailableSlotsByRound(int roundId)
    {
        var slots = await _slotRepository.GetAvailableSlotsByRound(roundId);
        return _mapper.Map<List<SlotDto>>(slots);
    }
}
