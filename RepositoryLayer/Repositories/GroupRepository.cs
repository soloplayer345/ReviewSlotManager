using RepositoryLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories;

public class GroupRepository : BaseRepository<Group>, IGroupRepository
{
    private readonly ReviewSlotDbContext _context;

    public GroupRepository(ReviewSlotDbContext context)
        : base(context, x => x.GroupId)
    {
        _context = context;
    }

    public Task<int> Count()
    {
        return _context.Groups.CountAsync();
    }

    public Task<List<Group>> GetBySemester(int semesterId)
    {
        return _context.Groups
            .AsNoTracking()
            .Where(x => x.SemesterId == semesterId)
            .OrderBy(x => x.GroupName)
            .ToListAsync();
    }
}
