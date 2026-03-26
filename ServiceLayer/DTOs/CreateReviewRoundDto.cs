using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.DTOs;

public class CreateReviewRoundDto
{
    [Required]
    public int SemesterId { get; set; }

    [Required]
    [Range(1, 3)]
    public int RoundNumber { get; set; }

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
}
