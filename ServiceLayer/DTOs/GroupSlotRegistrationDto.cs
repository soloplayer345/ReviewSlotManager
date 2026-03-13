namespace ServiceLayer.DTOs;

public class GroupSlotRegistrationDto
{
    public int RegistrationId { get; set; }
    public int GroupId { get; set; }
    public int SlotId { get; set; }
    public int RegisteredBy { get; set; }
    public DateTime RegisteredAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
