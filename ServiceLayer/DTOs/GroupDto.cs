namespace ServiceLayer.DTOs;

public class GroupDto
{
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public string ProjectTitle { get; set; } = string.Empty;
    public int SemesterId { get; set; }
    public int GvhdId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
