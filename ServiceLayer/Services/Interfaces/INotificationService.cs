using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces;

public interface INotificationService : IBaseService<Notification, NotificationDto>
{
    Task<int> Count(int? userId = null);
    Task<List<NotificationDto>> GetByUser(int userId, int pageSize = 20, int pageNumber = 1);
    Task<NotificationDto> Create(CreateNotificationDto dto);
    Task MarkAsRead(int notificationId);
    Task MarkAllAsRead(int userId);
}
