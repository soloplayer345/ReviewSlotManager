using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories.Interfaces;

public interface IGroupSlotRegistrationRepository
{
    Task<List<GroupSlotRegistration>> Read(int pageSize = 20, int pageNumber = 1);
    Task<GroupSlotRegistration?> GetById(int registrationId);
    Task<int> Count();
    Task<List<GroupSlotRegistration>> GetBySlot(int slotId);
    Task<List<GroupSlotRegistration>> GetByGroup(int groupId);
    Task<GroupSlotRegistration> Register(int groupId, int slotId, int userId);
    Task Cancel(int registrationId);
}
