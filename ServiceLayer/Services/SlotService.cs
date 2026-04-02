using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;
using ServiceLayer.Exceptions;

namespace ServiceLayer.Services;

public class SlotService : BaseService<Slot, SlotDto>, ISlotService
{
    private readonly ISlotRepository _slotRepository;
    private readonly IMapper _mapper;
    private readonly ReviewSlotDbContext _context;

    public SlotService(ISlotRepository slotRepository, IMapper mapper, ReviewSlotDbContext context)
        : base(slotRepository, mapper)
    {
        _slotRepository = slotRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<SlotDto>> GetAvailableSlotsByRound(int roundId)
    {
        var slots = await _slotRepository.GetAvailableSlotsByRound(roundId);
        return await BuildSlotDtos(slots);
    }

    public override async Task<List<SlotDto>> Read(int pageSize, int pageNumber)
    {
        var slots = await _slotRepository.Read(pageSize, pageNumber);
        return await BuildSlotDtos(slots);
    }

    public override async Task<SlotDto> Read(int id)
    {
        var slot = await _slotRepository.Read(id)
            ?? throw new KeyNotFoundException("Entity not found.");
        return (await BuildSlotDtos([slot]))[0];
    }

    public async Task<List<SlotDetailsDto>> GetDetailsByRound(int roundId)
    {
        var slots = await _slotRepository.GetAvailableSlotsByRound(roundId);
        if (slots.Count == 0)
        {
            return [];
        }

        var slotIds = slots.Select(x => x.SlotId).ToList();
        var groupCounts = await _context.GroupSlotRegistrations
            .AsNoTracking()
            .Where(x => slotIds.Contains(x.SlotId) && x.Status == RegistrationStatus.Registered)
            .GroupBy(x => x.SlotId)
            .Select(g => new { SlotId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SlotId, x => x.Count);

        var reviewerCounts = await _context.ReviewerSlotRegistrations
            .AsNoTracking()
            .Where(x => slotIds.Contains(x.SlotId) && x.Status == RegistrationStatus.Registered)
            .GroupBy(x => x.SlotId)
            .Select(g => new { SlotId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SlotId, x => x.Count);

        return slots
            .OrderBy(x => x.StartTime)
            .Select(slot => new SlotDetailsDto
            {
                SlotId = slot.SlotId,
                RoundId = slot.RoundId,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                Room = slot.Room,
                MaxGroups = slot.MaxGroups,
                CurrentGroupCount = groupCounts.GetValueOrDefault(slot.SlotId, 0),
                MinReviewers = slot.MinReviewers,
                MaxReviewers = slot.MaxReviewers,
                CurrentReviewerCount = reviewerCounts.GetValueOrDefault(slot.SlotId, 0),
                CreatedBy = slot.CreatedBy,
                Status = slot.Status.ToString()
            })
            .ToList();
    }

    public async Task<SlotDto> Create(CreateSlotDto dto)
    {
        var roundExists = await _context.ReviewRounds.AnyAsync(r => r.RoundId == dto.RoundId);
        if (!roundExists)
            throw new NotFoundException($"ReviewRound {dto.RoundId} not found.");

        var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.CreatedBy);
        if (!userExists)
            throw new NotFoundException($"User {dto.CreatedBy} not found.");

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
        return (await BuildSlotDtos([created]))[0];
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
        return (await BuildSlotDtos([existing]))[0];
    }

    private async Task<List<SlotDto>> BuildSlotDtos(List<Slot> slots)
    {
        if (slots.Count == 0)
        {
            return [];
        }

        var slotIds = slots.Select(x => x.SlotId).ToList();
        var groupCounts = await _context.GroupSlotRegistrations
            .AsNoTracking()
            .Where(x => slotIds.Contains(x.SlotId) && x.Status == RegistrationStatus.Registered)
            .GroupBy(x => x.SlotId)
            .Select(g => new { SlotId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SlotId, x => x.Count);

        var dtos = _mapper.Map<List<SlotDto>>(slots);
        foreach (var dto in dtos)
        {
            dto.CurrentGroupCount = groupCounts.GetValueOrDefault(dto.SlotId, 0);
        }

        return dtos;
    }
}
