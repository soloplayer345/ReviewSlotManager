namespace ServiceLayer.DTOs;

public class SlotDetailsDto
{
    public int SlotId { get; set; }
    public int RoundId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Room { get; set; } = string.Empty;
    public int MaxGroups { get; set; }
    public int CurrentGroupCount { get; set; }
    public int MinReviewers { get; set; }
    public int MaxReviewers { get; set; }
    public int CurrentReviewerCount { get; set; }
    public int CreatedBy { get; set; }
    public string Status { get; set; } = string.Empty;
}
