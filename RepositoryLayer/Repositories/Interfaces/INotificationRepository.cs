using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories.Interfaces;

public interface INotificationRepository : IBaseRepository<Notification>
{
    Task<int> Count(int? userId = null);
    Task<List<Notification>> GetByUser(int userId, int pageSize = 20, int pageNumber = 1);
    Task MarkAsRead(int notificationId);
    Task MarkAllAsRead(int userId);
}
