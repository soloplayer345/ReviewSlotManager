using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces;

public interface IReviewerSlotRegistrationService
{
    Task<List<ReviewerSlotRegistrationDto>> Read(int pageSize, int pageNumber);
    Task<ReviewerSlotRegistrationDto> GetById(int reviewerRegistrationId);
    Task<int> Count();
    Task<List<ReviewerSlotRegistrationDto>> GetBySlot(int slotId);
    Task<List<ReviewerSlotRegistrationDto>> GetByReviewer(int reviewerId);
    Task<ReviewerSlotRegistrationDto> Register(CreateReviewerSlotRegistrationDto dto);
    Task Cancel(int registrationId);
}
