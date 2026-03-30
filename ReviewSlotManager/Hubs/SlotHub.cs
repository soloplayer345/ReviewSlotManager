using Microsoft.AspNetCore.SignalR;

namespace ReviewSlotManager.Hubs;

public class SlotHub : Hub
{
    public Task JoinRound(string roundKey)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, roundKey);
    }

    public Task LeaveRound(string roundKey)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, roundKey);
    }

    public static string BuildRoundKey(int roundId) => $"round-{roundId}";
}
