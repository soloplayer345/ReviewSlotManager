namespace ServiceLayer.Services.Interfaces;

public interface ISlotRealtimeNotifier
{
    Task PublishSlotStatusChanged(int slotId);
}
