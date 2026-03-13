using RepositoryLayer.Enums;

namespace RepositoryLayer.Entities;

public class GroupSlotRegistration
{
    public int RegistrationId { get; set; }
    public int GroupId { get; set; }
    public int SlotId { get; set; }
    public DateTime RegisteredAt { get; set; }
    public int RegisteredBy { get; set; }
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Registered;
}
