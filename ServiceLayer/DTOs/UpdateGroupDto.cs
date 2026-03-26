using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.DTOs;

public class UpdateGroupDto
{
    [Required]
    [MaxLength(100)]
    public string GroupName { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string ProjectTitle { get; set; } = string.Empty;

    [Required]
    public int GvhdId { get; set; }
}
