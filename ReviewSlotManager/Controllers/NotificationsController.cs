using ServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

public class NotificationsController : BaseController<INotificationService, NotificationDto>
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service) : base(service)
    {
        _service = service;
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count([FromQuery] int? userId)
    {
        var count = await _service.Count(userId);
        return Ok(new { count });
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId, [FromQuery] int pageSize = 20, [FromQuery] int pageNumber = 1)
    {
        var result = await _service.GetByUser(userId, pageSize, pageNumber);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationDto dto)
    {
        var result = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.NotificationId }, result);
    }

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _service.MarkAsRead(id);
        return NoContent();
    }

    [HttpPut("user/{userId:int}/read-all")]
    public async Task<IActionResult> MarkAllAsRead(int userId)
    {
        await _service.MarkAllAsRead(userId);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return NoContent();
    }
}
