using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.DTOs;

public class UpdateSlotDto
{
    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    [MaxLength(200)]
    public string Room { get; set; } = string.Empty;

    [Range(1, 10)]
    public int MaxGroups { get; set; } = 3;

    [Range(1, 10)]
    public int MinReviewers { get; set; } = 1;

    [Range(1, 10)]
    public int MaxReviewers { get; set; } = 3;

    [Required]
    public string Status { get; set; } = string.Empty;
}
