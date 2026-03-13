namespace RepositoryLayer.Entities;

public class Group
{
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public string ProjectTitle { get; set; } = string.Empty;
    public int SemesterId { get; set; }
    public int GvhdId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
