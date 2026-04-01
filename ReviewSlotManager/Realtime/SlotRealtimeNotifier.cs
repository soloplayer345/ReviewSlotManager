using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Enums;
using ReviewSlotManager.Hubs;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;

namespace ReviewSlotManager.Realtime;

public class SlotRealtimeNotifier : ISlotRealtimeNotifier
{
    private readonly ReviewSlotDbContext _context;
    private readonly IHubContext<SlotHub> _hubContext;

    public SlotRealtimeNotifier(ReviewSlotDbContext context, IHubContext<SlotHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task PublishSlotStatusChanged(int slotId)
    {
        var slot = await _context.Slots
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SlotId == slotId);

        if (slot is null)
        {
            return;
        }

        var currentGroupCount = await _context.GroupSlotRegistrations
            .AsNoTracking()
            .CountAsync(x => x.SlotId == slotId && x.Status == RegistrationStatus.Registered);

        var payload = new SlotRealtimeStatusDto
        {
            SlotId = slot.SlotId,
            RoundId = slot.RoundId,
            CurrentGroupCount = currentGroupCount,
            MaxGroups = slot.MaxGroups,
            AvailabilityStatus = ResolveAvailabilityStatus(currentGroupCount, slot.MaxGroups)
        };

        var roundKey = SlotHub.BuildRoundKey(slot.RoundId);
        await _hubContext.Clients.Group(roundKey).SendAsync("slotStatusChanged", payload);
    }

    private static string ResolveAvailabilityStatus(int currentGroupCount, int maxGroups)
    {
        if (currentGroupCount <= 0)
        {
            return "còn trống";
        }

        if (currentGroupCount >= maxGroups)
        {
            return "đã hết chỗ";
        }

        return "đang có nhóm đặt";
    }
}
