using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.DTOs;

public class CreateGroupMemberDto
{
    [Required]
    public int GroupId { get; set; }

    [Required]
    public int StudentId { get; set; }
}
