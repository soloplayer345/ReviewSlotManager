namespace ServiceLayer.DTOs;

public class CreateGroupSlotRegistrationDto
{
    public int GroupId { get; set; }
    public int SlotId { get; set; }
    public int RegisteredBy { get; set; }
}
