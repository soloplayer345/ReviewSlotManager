using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public interface IGroupSlotRegistrationService
{
    Task<List<GroupSlotRegistrationDto>> Read(int pageSize, int pageNumber);
    Task<GroupSlotRegistrationDto> Register(CreateGroupSlotRegistrationDto dto);
    Task Cancel(int registrationId);
}
