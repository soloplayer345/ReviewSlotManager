namespace ServiceLayer.DTOs;

public class SlotRealtimeStatusDto
{
    public int SlotId { get; set; }
    public int RoundId { get; set; }
    public int CurrentGroupCount { get; set; }
    public int MaxGroups { get; set; }
    public string AvailabilityStatus { get; set; } = string.Empty;
}
