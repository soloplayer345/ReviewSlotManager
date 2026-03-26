using RepositoryLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories;

public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
    private readonly ReviewSlotDbContext _context;

    public NotificationRepository(ReviewSlotDbContext context)
        : base(context, x => x.NotificationId)
    {
        _context = context;
    }

    public Task<int> Count(int? userId = null)
    {
        var query = _context.Notifications.AsQueryable();
        if (userId.HasValue)
            query = query.Where(x => x.UserId == userId.Value);
        return query.CountAsync();
    }

    public Task<List<Notification>> GetByUser(int userId, int pageSize = 20, int pageNumber = 1)
    {
        var skip = Math.Max(0, (pageNumber - 1) * pageSize);
        return _context.Notifications
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task MarkAsRead(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification is null)
            throw new KeyNotFoundException($"Notification {notificationId} not found.");

        notification.IsRead = true;
        await _context.SaveChangesAsync();
    }

    public async Task MarkAllAsRead(int userId)
    {
        await _context.Notifications
            .Where(x => x.UserId == userId && !x.IsRead)
            .ExecuteUpdateAsync(x => x.SetProperty(n => n.IsRead, true));
    }
}
