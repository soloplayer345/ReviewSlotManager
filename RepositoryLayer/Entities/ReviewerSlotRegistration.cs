using RepositoryLayer.Enums;

namespace RepositoryLayer.Entities;

public class ReviewerSlotRegistration
{
    public int ReviewerRegistrationId { get; set; }
    public int ReviewerId { get; set; }
    public int SlotId { get; set; }
    public DateTime RegisteredAt { get; set; }
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Registered;
}
