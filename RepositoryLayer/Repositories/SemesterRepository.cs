using RepositoryLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories;

public class SemesterRepository : BaseRepository<Semester>, ISemesterRepository
{
    private readonly ReviewSlotDbContext _context;

    public SemesterRepository(ReviewSlotDbContext context)
        : base(context, x => x.SemesterId)
    {
        _context = context;
    }

    public Task<int> Count()
    {
        return _context.Semesters.CountAsync();
    }

    public Task<Semester?> GetActiveSemester()
    {
        return _context.Semesters
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsActive);
    }
}
