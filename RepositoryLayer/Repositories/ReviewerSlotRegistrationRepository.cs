using RepositoryLayer.Data;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;

namespace RepositoryLayer.Repositories;

public class ReviewerSlotRegistrationRepository
{
    private readonly InMemoryDataStore _store;

    public ReviewerSlotRegistrationRepository(InMemoryDataStore store)
    {
        _store = store;
    }

    public Task<List<ReviewerSlotRegistration>> Read(int pageSize = 20, int pageNumber = 1)
    {
        var skip = Math.Max(0, (pageNumber - 1) * pageSize);
        var result = _store.ReviewerSlotRegistrations.Skip(skip).Take(pageSize).ToList();
        return Task.FromResult(result);
    }

    public Task<ReviewerSlotRegistration> Register(int reviewerId, int slotId)
    {
        lock (_store.SyncRoot)
        {
            var reviewer = _store.Users.FirstOrDefault(x => x.UserId == reviewerId)
                ?? throw new KeyNotFoundException("Reviewer not found.");
            if (reviewer.Role != UserRole.GvReview)
            {
                throw new InvalidOperationException("User is not a GV Review.");
            }

            var slot = _store.Slots.FirstOrDefault(x => x.SlotId == slotId)
                ?? throw new KeyNotFoundException("Slot not found.");

            var round = _store.ReviewRounds.FirstOrDefault(x => x.RoundId == slot.RoundId)
                ?? throw new KeyNotFoundException("Round not found.");

            var config = _store.ReviewerSlotConfigs.FirstOrDefault(x => x.RoundId == round.RoundId)
                ?? throw new InvalidOperationException("Reviewer slot config not found.");

            var duplicate = _store.ReviewerSlotRegistrations.Any(x =>
                x.ReviewerId == reviewerId &&
                x.SlotId == slotId &&
                x.Status == RegistrationStatus.Registered);
            if (duplicate)
            {
                throw new InvalidOperationException("Reviewer already registered this slot.");
            }

            var currentCountByRound = _store.ReviewerSlotRegistrations.Count(x =>
                x.ReviewerId == reviewerId &&
                x.Status == RegistrationStatus.Registered &&
                _store.Slots.Any(s => s.SlotId == x.SlotId && s.RoundId == round.RoundId));
            if (currentCountByRound >= config.MaxSlots)
            {
                throw new InvalidOperationException("Reviewer reached max slot limit for this round.");
            }

            var groupIdsInSlot = _store.GroupSlotRegistrations
                .Where(x => x.SlotId == slotId && x.Status == RegistrationStatus.Registered)
                .Select(x => x.GroupId)
                .ToList();

            var hasConflict = _store.Groups.Any(x => groupIdsInSlot.Contains(x.GroupId) && x.GvhdId == reviewerId);
            if (hasConflict)
            {
                throw new InvalidOperationException("Reviewer cannot review their own supervised group.");
            }

            var registration = new ReviewerSlotRegistration
            {
                ReviewerRegistrationId = _store.NextId(_store.ReviewerSlotRegistrations, x => x.ReviewerRegistrationId),
                ReviewerId = reviewerId,
                SlotId = slotId,
                RegisteredAt = DateTime.UtcNow,
                Status = RegistrationStatus.Registered
            };

            _store.ReviewerSlotRegistrations.Add(registration);
            return Task.FromResult(registration);
        }
    }

    public Task Cancel(int reviewerRegistrationId)
    {
        lock (_store.SyncRoot)
        {
            var registration = _store.ReviewerSlotRegistrations.FirstOrDefault(x => x.ReviewerRegistrationId == reviewerRegistrationId)
                ?? throw new KeyNotFoundException("Reviewer registration not found.");

            registration.Status = RegistrationStatus.Cancelled;
            return Task.CompletedTask;
        }
    }
}
