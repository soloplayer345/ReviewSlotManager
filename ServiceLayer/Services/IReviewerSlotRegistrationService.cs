using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public interface IReviewerSlotRegistrationService
{
    Task<List<ReviewerSlotRegistrationDto>> Read(int pageSize, int pageNumber);
    Task<ReviewerSlotRegistrationDto> Register(CreateReviewerSlotRegistrationDto dto);
    Task Cancel(int registrationId);
}
