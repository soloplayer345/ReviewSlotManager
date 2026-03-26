using RepositoryLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly ReviewSlotDbContext _context;

    public UserRepository(ReviewSlotDbContext context)
        : base(context, x => x.UserId)
    {
        _context = context;
    }

    public Task<int> Count()
    {
        return _context.Users.CountAsync();
    }

    public Task<User?> GetByEmail(string email)
    {
        return _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }
}
