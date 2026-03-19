using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories;

public interface IReviewerSlotRegistrationRepository
{
    Task<List<ReviewerSlotRegistration>> Read(int pageSize = 20, int pageNumber = 1);
    Task<ReviewerSlotRegistration> Register(int reviewerId, int slotId);
    Task Cancel(int reviewerRegistrationId);
}
