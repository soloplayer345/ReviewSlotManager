using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.DTOs;

public class UpdateReviewRoundDto
{
    [Required]
    [MaxLength(100)]
    public string RoundName { get; set; } = string.Empty;

    [Required]
    public DateTime RegistrationOpenAt { get; set; }

    [Required]
    public DateTime RegistrationCloseAt { get; set; }

    [Required]
    public DateTime ReviewDateFrom { get; set; }

    [Required]
    public DateTime ReviewDateTo { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;
}
