using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;
using AutoMapper;
using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public class ReviewerSlotConfigService : BaseService<ReviewerSlotConfig, ReviewerSlotConfigDto>, IReviewerSlotConfigService
{
    private readonly IReviewerSlotConfigRepository _configRepository;
    private readonly IMapper _mapper;

    public ReviewerSlotConfigService(IReviewerSlotConfigRepository configRepository, IMapper mapper)
        : base(configRepository, mapper)
    {
        _configRepository = configRepository;
        _mapper = mapper;
    }

    public async Task<ReviewerSlotConfigDto?> GetByRound(int roundId)
    {
        var config = await _configRepository.GetByRound(roundId);
        return config is null ? null : _mapper.Map<ReviewerSlotConfigDto>(config);
    }

    public async Task<ReviewerSlotConfigDto> Create(CreateReviewerSlotConfigDto dto)
    {
        var existing = await _configRepository.GetByRound(dto.RoundId);
        if (existing is not null)
            throw new InvalidOperationException($"Config for round {dto.RoundId} already exists.");

        var entity = _mapper.Map<ReviewerSlotConfig>(dto);
        entity.UpdatedAt = DateTime.UtcNow;
        var created = await _configRepository.Create(entity);
        return _mapper.Map<ReviewerSlotConfigDto>(created);
    }

    public async Task<ReviewerSlotConfigDto> Update(int id, UpdateReviewerSlotConfigDto dto)
    {
        var existing = await _configRepository.Read(id)
            ?? throw new KeyNotFoundException($"ReviewerSlotConfig {id} not found.");
        existing.MinSlots = dto.MinSlots;
        existing.MaxSlots = dto.MaxSlots;
        existing.UpdatedBy = dto.UpdatedBy;
        existing.UpdatedAt = DateTime.UtcNow;
        await _configRepository.Update(existing);
        return _mapper.Map<ReviewerSlotConfigDto>(existing);
    }
}
