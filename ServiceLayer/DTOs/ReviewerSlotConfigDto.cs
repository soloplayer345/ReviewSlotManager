namespace ServiceLayer.DTOs;

public class ReviewerSlotConfigDto
{
    public int ConfigId { get; set; }
    public int RoundId { get; set; }
    public int MinSlots { get; set; }
    public int MaxSlots { get; set; }
    public int UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}
