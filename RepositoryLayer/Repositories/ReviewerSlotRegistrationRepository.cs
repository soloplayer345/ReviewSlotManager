using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;

namespace RepositoryLayer.Repositories;

public class ReviewerSlotRegistrationRepository : IReviewerSlotRegistrationRepository
{
    private readonly ReviewSlotDbContext _context;

    public ReviewerSlotRegistrationRepository(ReviewSlotDbContext context)
    {
        _context = context;
    }

    public Task<List<ReviewerSlotRegistration>> Read(int pageSize = 20, int pageNumber = 1)
    {
        var skip = Math.Max(0, (pageNumber - 1) * pageSize);
        return _context.ReviewerSlotRegistrations
            .AsNoTracking()
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<ReviewerSlotRegistration> Register(int reviewerId, int slotId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var reviewer = await _context.Users.FirstOrDefaultAsync(x => x.UserId == reviewerId)
            ?? throw new KeyNotFoundException("Reviewer not found.");
        if (reviewer.Role != UserRole.GvReview)
        {
            throw new InvalidOperationException("User is not a GV Review.");
        }

        var slot = await _context.Slots.FirstOrDefaultAsync(x => x.SlotId == slotId)
            ?? throw new KeyNotFoundException("Slot not found.");

        var round = await _context.ReviewRounds.FirstOrDefaultAsync(x => x.RoundId == slot.RoundId)
            ?? throw new KeyNotFoundException("Round not found.");

        var config = await _context.ReviewerSlotConfigs.FirstOrDefaultAsync(x => x.RoundId == round.RoundId)
            ?? throw new InvalidOperationException("Reviewer slot config not found.");

        var duplicate = await _context.ReviewerSlotRegistrations.AnyAsync(x =>
            x.ReviewerId == reviewerId &&
            x.SlotId == slotId &&
            x.Status == RegistrationStatus.Registered);
        if (duplicate)
        {
            throw new InvalidOperationException("Reviewer already registered this slot.");
        }

        var currentCountByRound = await _context.ReviewerSlotRegistrations.CountAsync(x =>
            x.ReviewerId == reviewerId &&
            x.Status == RegistrationStatus.Registered &&
            _context.Slots.Any(s => s.SlotId == x.SlotId && s.RoundId == round.RoundId));
        if (currentCountByRound >= config.MaxSlots)
        {
            throw new InvalidOperationException("Reviewer reached max slot limit for this round.");
        }

        var groupIdsInSlot = await _context.GroupSlotRegistrations
            .Where(x => x.SlotId == slotId && x.Status == RegistrationStatus.Registered)
            .Select(x => x.GroupId)
            .ToListAsync();

        var hasConflict = await _context.Groups.AnyAsync(x => groupIdsInSlot.Contains(x.GroupId) && x.GvhdId == reviewerId);
        if (hasConflict)
        {
            throw new InvalidOperationException("Reviewer cannot review their own supervised group.");
        }

        var registration = new ReviewerSlotRegistration
        {
            ReviewerId = reviewerId,
            SlotId = slotId,
            RegisteredAt = DateTime.UtcNow,
            Status = RegistrationStatus.Registered
        };

        await _context.ReviewerSlotRegistrations.AddAsync(registration);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return registration;
    }

    public async Task Cancel(int reviewerRegistrationId)
    {
        var registration = await _context.ReviewerSlotRegistrations.FirstOrDefaultAsync(x => x.ReviewerRegistrationId == reviewerRegistrationId)
            ?? throw new KeyNotFoundException("Reviewer registration not found.");

        registration.Status = RegistrationStatus.Cancelled;
        await _context.SaveChangesAsync();
    }
}
