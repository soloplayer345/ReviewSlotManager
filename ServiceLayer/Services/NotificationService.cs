using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;
using AutoMapper;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public class NotificationService : BaseService<Notification, NotificationDto>, INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;

    public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
        : base(notificationRepository, mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public Task<int> Count(int? userId = null)
    {
        return _notificationRepository.Count(userId);
    }

    public async Task<List<NotificationDto>> GetByUser(int userId, int pageSize = 20, int pageNumber = 1)
    {
        var notifications = await _notificationRepository.GetByUser(userId, pageSize, pageNumber);
        return _mapper.Map<List<NotificationDto>>(notifications);
    }

    public async Task<NotificationDto> Create(CreateNotificationDto dto)
    {
        if (!Enum.TryParse<NotificationType>(dto.Type, true, out var type))
            throw new ArgumentException($"Invalid notification type '{dto.Type}'.");

        var entity = new Notification
        {
            UserId = dto.UserId,
            Title = dto.Title,
            Message = dto.Message,
            Type = type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _notificationRepository.Create(entity);
        return _mapper.Map<NotificationDto>(created);
    }

    public Task MarkAsRead(int notificationId)
    {
        return _notificationRepository.MarkAsRead(notificationId);
    }

    public Task MarkAllAsRead(int userId)
    {
        return _notificationRepository.MarkAllAsRead(userId);
    }
}
