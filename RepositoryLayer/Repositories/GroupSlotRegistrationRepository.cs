using RepositoryLayer.Data;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;

namespace RepositoryLayer.Repositories;

public class GroupSlotRegistrationRepository
{
    private readonly InMemoryDataStore _store;

    public GroupSlotRegistrationRepository(InMemoryDataStore store)
    {
        _store = store;
    }

    public Task<List<GroupSlotRegistration>> Read(int pageSize = 20, int pageNumber = 1)
    {
        var skip = Math.Max(0, (pageNumber - 1) * pageSize);
        var result = _store.GroupSlotRegistrations.Skip(skip).Take(pageSize).ToList();
        return Task.FromResult(result);
    }

    public Task<GroupSlotRegistration> Register(int groupId, int slotId, int userId)
    {
        lock (_store.SyncRoot)
        {
            var slot = _store.Slots.FirstOrDefault(x => x.SlotId == slotId)
                ?? throw new KeyNotFoundException("Slot not found.");

            var round = _store.ReviewRounds.FirstOrDefault(x => x.RoundId == slot.RoundId)
                ?? throw new KeyNotFoundException("Round not found.");

            var now = DateTime.UtcNow;
            if (round.Status != ReviewRoundStatus.Open || now < round.RegistrationOpenAt || now > round.RegistrationCloseAt)
            {
                throw new InvalidOperationException("Round is not open for registration.");
            }

            if (slot.Status is SlotStatus.Locked or SlotStatus.Cancelled)
            {
                throw new InvalidOperationException("This slot is not available.");
            }

            var activeCount = _store.GroupSlotRegistrations.Count(x => x.SlotId == slotId && x.Status == RegistrationStatus.Registered);
            if (activeCount >= slot.MaxGroups)
            {
                slot.Status = SlotStatus.Full;
                throw new InvalidOperationException("Slot is full.");
            }

            var duplicated = _store.GroupSlotRegistrations.Any(x =>
                x.GroupId == groupId &&
                x.SlotId == slotId &&
                x.Status == RegistrationStatus.Registered);
            if (duplicated)
            {
                throw new InvalidOperationException("Group already registered this slot.");
            }

            var registration = new GroupSlotRegistration
            {
                RegistrationId = _store.NextId(_store.GroupSlotRegistrations, x => x.RegistrationId),
                GroupId = groupId,
                SlotId = slotId,
                RegisteredBy = userId,
                RegisteredAt = now,
                Status = RegistrationStatus.Registered
            };

            _store.GroupSlotRegistrations.Add(registration);

            var newCount = activeCount + 1;
            slot.Status = newCount >= slot.MaxGroups ? SlotStatus.Full : SlotStatus.Open;

            return Task.FromResult(registration);
        }
    }

    public Task Cancel(int registrationId)
    {
        lock (_store.SyncRoot)
        {
            var registration = _store.GroupSlotRegistrations.FirstOrDefault(x => x.RegistrationId == registrationId)
                ?? throw new KeyNotFoundException("Registration not found.");

            if (registration.Status == RegistrationStatus.Cancelled)
            {
                return Task.CompletedTask;
            }

            var slot = _store.Slots.First(x => x.SlotId == registration.SlotId);
            var round = _store.ReviewRounds.First(x => x.RoundId == slot.RoundId);

            if (round.Status is ReviewRoundStatus.Closed or ReviewRoundStatus.Completed || slot.Status == SlotStatus.Locked)
            {
                throw new InvalidOperationException("Cannot cancel registration at this time.");
            }

            registration.Status = RegistrationStatus.Cancelled;
            if (slot.Status == SlotStatus.Full)
            {
                slot.Status = SlotStatus.Open;
            }

            return Task.CompletedTask;
        }
    }
}
