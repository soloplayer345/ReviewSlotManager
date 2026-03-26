using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;
using AutoMapper;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;
using ServiceLayer.Exceptions;

namespace ServiceLayer.Services;

public class ReviewerSlotRegistrationService : IReviewerSlotRegistrationService
{
    private readonly IReviewerSlotRegistrationRepository _repository;
    private readonly IMapper _mapper;

    public ReviewerSlotRegistrationService(IReviewerSlotRegistrationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ReviewerSlotRegistrationDto>> Read(int pageSize, int pageNumber)
    {
        var entities = await _repository.Read(pageSize, pageNumber);
        return _mapper.Map<List<ReviewerSlotRegistrationDto>>(entities);
    }

    public async Task<ReviewerSlotRegistrationDto> GetById(int reviewerRegistrationId)
    {
        var entity = await _repository.GetById(reviewerRegistrationId)
            ?? throw new NotFoundException($"Reviewer registration {reviewerRegistrationId} not found.");
        return _mapper.Map<ReviewerSlotRegistrationDto>(entity);
    }

    public Task<int> Count()
    {
        return _repository.Count();
    }

    public async Task<List<ReviewerSlotRegistrationDto>> GetBySlot(int slotId)
    {
        var entities = await _repository.GetBySlot(slotId);
        return _mapper.Map<List<ReviewerSlotRegistrationDto>>(entities);
    }

    public async Task<List<ReviewerSlotRegistrationDto>> GetByReviewer(int reviewerId)
    {
        var entities = await _repository.GetByReviewer(reviewerId);
        return _mapper.Map<List<ReviewerSlotRegistrationDto>>(entities);
    }

    public async Task<ReviewerSlotRegistrationDto> Register(CreateReviewerSlotRegistrationDto dto)
    {
        try
        {
            var entity = await _repository.Register(dto.ReviewerId, dto.SlotId);
            return _mapper.Map<ReviewerSlotRegistrationDto>(entity);
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            throw new BusinessRuleException(ex.Message);
        }
    }

    public async Task Cancel(int registrationId)
    {
        try
        {
            await _repository.Cancel(registrationId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            throw new BusinessRuleException(ex.Message);
        }
    }
}
