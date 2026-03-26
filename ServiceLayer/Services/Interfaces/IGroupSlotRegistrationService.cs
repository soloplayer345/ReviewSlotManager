using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces;

public interface IGroupSlotRegistrationService
{
    Task<List<GroupSlotRegistrationDto>> Read(int pageSize, int pageNumber);
    Task<GroupSlotRegistrationDto> GetById(int registrationId);
    Task<int> Count();
    Task<List<GroupSlotRegistrationDto>> GetBySlot(int slotId);
    Task<List<GroupSlotRegistrationDto>> GetByGroup(int groupId);
    Task<GroupSlotRegistrationDto> Register(CreateGroupSlotRegistrationDto dto);
    Task Cancel(int registrationId);
}
