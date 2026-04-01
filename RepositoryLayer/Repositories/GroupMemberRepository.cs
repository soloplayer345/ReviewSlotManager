using RepositoryLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories;

public class GroupMemberRepository : BaseRepository<GroupMember>, IGroupMemberRepository
{
    private readonly ReviewSlotDbContext _context;

    public GroupMemberRepository(ReviewSlotDbContext context)
        : base(context, x => x.MemberId)
    {
        _context = context;
    }

    public Task<int> Count()
    {
        return _context.GroupMembers.CountAsync();
    }

    public Task<List<GroupMember>> GetByGroup(int groupId)
    {
        return _context.GroupMembers
            .AsNoTracking()
            .Where(x => x.GroupId == groupId)
            .ToListAsync();
    }

    public Task<List<GroupMember>> GetByStudent(int studentId)
    {
        return _context.GroupMembers
            .AsNoTracking()
            .Where(x => x.StudentId == studentId)
            .OrderByDescending(x => x.JoinedAt)
            .ToListAsync();
    }

    public async Task<GroupMember?> GetByStudentAndSemester(int studentId, int semesterId)
    {
        return await _context.GroupMembers
            .AsNoTracking()
            .Join(_context.Groups,
                gm => gm.GroupId,
                g => g.GroupId,
                (gm, g) => new { gm, g })
            .Where(x => x.gm.StudentId == studentId && x.g.SemesterId == semesterId)
            .Select(x => x.gm)
            .FirstOrDefaultAsync();
    }
}
