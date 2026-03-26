using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;
using AutoMapper;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;
using ServiceLayer.Exceptions;

namespace ServiceLayer.Services;

public class GroupSlotRegistrationService : IGroupSlotRegistrationService
{
    private readonly IGroupSlotRegistrationRepository _repository;
    private readonly IMapper _mapper;

    public GroupSlotRegistrationService(IGroupSlotRegistrationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<GroupSlotRegistrationDto>> Read(int pageSize, int pageNumber)
    {
        var entities = await _repository.Read(pageSize, pageNumber);
        return _mapper.Map<List<GroupSlotRegistrationDto>>(entities);
    }

    public async Task<GroupSlotRegistrationDto> GetById(int registrationId)
    {
        var entity = await _repository.GetById(registrationId)
            ?? throw new NotFoundException($"Registration {registrationId} not found.");
        return _mapper.Map<GroupSlotRegistrationDto>(entity);
    }

    public Task<int> Count()
    {
        return _repository.Count();
    }

    public async Task<List<GroupSlotRegistrationDto>> GetBySlot(int slotId)
    {
        var entities = await _repository.GetBySlot(slotId);
        return _mapper.Map<List<GroupSlotRegistrationDto>>(entities);
    }

    public async Task<List<GroupSlotRegistrationDto>> GetByGroup(int groupId)
    {
        var entities = await _repository.GetByGroup(groupId);
        return _mapper.Map<List<GroupSlotRegistrationDto>>(entities);
    }

    public async Task<GroupSlotRegistrationDto> Register(CreateGroupSlotRegistrationDto dto)
    {
        try
        {
            var entity = await _repository.Register(dto.GroupId, dto.SlotId, dto.RegisteredBy);
            return _mapper.Map<GroupSlotRegistrationDto>(entity);
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
