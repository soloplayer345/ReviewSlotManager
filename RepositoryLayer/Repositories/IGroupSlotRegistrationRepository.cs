using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories;

public interface IGroupSlotRegistrationRepository
{
    Task<List<GroupSlotRegistration>> Read(int pageSize = 20, int pageNumber = 1);
    Task<GroupSlotRegistration> Register(int groupId, int slotId, int userId);
    Task Cancel(int registrationId);
}
