using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.DTOs;

public class CreateNotificationDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;
}
