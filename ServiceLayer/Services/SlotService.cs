using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;
using AutoMapper;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public class SlotService : BaseService<Slot, SlotDto>, ISlotService
{
    private readonly ISlotRepository _slotRepository;
    private readonly IMapper _mapper;

    public SlotService(ISlotRepository slotRepository, IMapper mapper)
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

    public async Task<SlotDto> Create(CreateSlotDto dto)
    {
        var entity = new Slot
        {
            RoundId = dto.RoundId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Room = dto.Room,
            MaxGroups = dto.MaxGroups,
            MinReviewers = dto.MinReviewers,
            MaxReviewers = dto.MaxReviewers,
            Status = SlotStatus.Open,
            CreatedBy = dto.CreatedBy
        };

        var created = await _slotRepository.Create(entity);
        return _mapper.Map<SlotDto>(created);
    }

    public async Task<SlotDto> Update(int id, UpdateSlotDto dto)
    {
        var existing = await _slotRepository.Read(id)
            ?? throw new KeyNotFoundException($"Slot {id} not found.");

        if (!Enum.TryParse<SlotStatus>(dto.Status, true, out var status))
            throw new ArgumentException($"Invalid slot status '{dto.Status}'.");

        existing.StartTime = dto.StartTime;
        existing.EndTime = dto.EndTime;
        existing.Room = dto.Room;
        existing.MaxGroups = dto.MaxGroups;
        existing.MinReviewers = dto.MinReviewers;
        existing.MaxReviewers = dto.MaxReviewers;
        existing.Status = status;

        await _slotRepository.Update(existing);
        return _mapper.Map<SlotDto>(existing);
    }
}
