namespace ServiceLayer.DTOs;

public class ReviewRoundDto
{
    public int RoundId { get; set; }
    public int SemesterId { get; set; }
    public int RoundNumber { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public DateTime RegistrationOpenAt { get; set; }
    public DateTime RegistrationCloseAt { get; set; }
    public DateTime ReviewDateFrom { get; set; }
    public DateTime ReviewDateTo { get; set; }
    public string Status { get; set; } = string.Empty;
}
