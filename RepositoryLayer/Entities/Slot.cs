using RepositoryLayer.Enums;

namespace RepositoryLayer.Entities;

public class Slot
{
    public int SlotId { get; set; }
    public int RoundId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Room { get; set; } = string.Empty;
    public int MaxGroups { get; set; } = 3;
    public int MinReviewers { get; set; } = 1;
    public int MaxReviewers { get; set; } = 3;
    public SlotStatus Status { get; set; } = SlotStatus.Open;
    public int CreatedBy { get; set; }
}
