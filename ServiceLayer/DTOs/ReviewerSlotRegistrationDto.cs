namespace ServiceLayer.DTOs;

public class ReviewerSlotRegistrationDto
{
    public int ReviewerRegistrationId { get; set; }
    public int ReviewerId { get; set; }
    public int SlotId { get; set; }
    public DateTime RegisteredAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
