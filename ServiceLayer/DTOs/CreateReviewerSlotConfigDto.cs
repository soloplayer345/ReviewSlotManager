using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.DTOs;

public class CreateReviewerSlotConfigDto
{
    [Required]
    public int RoundId { get; set; }

    [Required]
    [Range(0, 100)]
    public int MinSlots { get; set; }

    [Required]
    [Range(1, 100)]
    public int MaxSlots { get; set; }

    [Required]
    public int UpdatedBy { get; set; }
}
