using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories.Interfaces;

public interface IReviewerSlotRegistrationRepository
{
    Task<List<ReviewerSlotRegistration>> Read(int pageSize = 20, int pageNumber = 1);
    Task<ReviewerSlotRegistration?> GetById(int reviewerRegistrationId);
    Task<int> Count();
    Task<List<ReviewerSlotRegistration>> GetBySlot(int slotId);
    Task<List<ReviewerSlotRegistration>> GetByReviewer(int reviewerId);
    Task<ReviewerSlotRegistration> Register(int reviewerId, int slotId);
    Task Cancel(int reviewerRegistrationId);
}
