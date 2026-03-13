namespace RepositoryLayer.Entities;

public class GroupMember
{
    public int MemberId { get; set; }
    public int GroupId { get; set; }
    public int StudentId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
