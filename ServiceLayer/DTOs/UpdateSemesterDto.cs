using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.DTOs;

public class UpdateSemesterDto
{
    [Required]
    [MaxLength(100)]
    public string SemesterName { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }
}
