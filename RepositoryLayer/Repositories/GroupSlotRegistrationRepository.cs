using RepositoryLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;

namespace RepositoryLayer.Repositories;

public class GroupSlotRegistrationRepository : IGroupSlotRegistrationRepository
{
    private readonly ReviewSlotDbContext _context;

    public GroupSlotRegistrationRepository(ReviewSlotDbContext context)
    {
        _context = context;
    }

    public Task<List<GroupSlotRegistration>> Read(int pageSize = 20, int pageNumber = 1)
    {
        var skip = Math.Max(0, (pageNumber - 1) * pageSize);
        return _context.GroupSlotRegistrations
            .AsNoTracking()
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<GroupSlotRegistration?> GetById(int registrationId)
    {
        return _context.GroupSlotRegistrations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RegistrationId == registrationId);
    }

    public Task<int> Count()
    {
        return _context.GroupSlotRegistrations.CountAsync();
    }

    public Task<List<GroupSlotRegistration>> GetBySlot(int slotId)
    {
        return _context.GroupSlotRegistrations
            .AsNoTracking()
            .Where(x => x.SlotId == slotId && x.Status == RegistrationStatus.Registered)
            .ToListAsync();
    }

    public Task<List<GroupSlotRegistration>> GetByGroup(int groupId)
    {
        return _context.GroupSlotRegistrations
            .AsNoTracking()
            .Where(x => x.GroupId == groupId)
            .OrderByDescending(x => x.RegisteredAt)
            .ToListAsync();
    }

    public async Task<GroupSlotRegistration> Register(int groupId, int slotId, int userId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var slot = await _context.Slots.FirstOrDefaultAsync(x => x.SlotId == slotId)
            ?? throw new KeyNotFoundException("Slot not found.");

        var round = await _context.ReviewRounds.FirstOrDefaultAsync(x => x.RoundId == slot.RoundId)
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

        var activeCount = await _context.GroupSlotRegistrations.CountAsync(x => x.SlotId == slotId && x.Status == RegistrationStatus.Registered);
        if (activeCount >= slot.MaxGroups)
        {
            slot.Status = SlotStatus.Full;
            await _context.SaveChangesAsync();
            throw new InvalidOperationException("Slot is full.");
        }

        var duplicated = await _context.GroupSlotRegistrations.AnyAsync(x =>
            x.GroupId == groupId &&
            x.SlotId == slotId &&
            x.Status == RegistrationStatus.Registered);
        if (duplicated)
        {
            throw new InvalidOperationException("Group already registered this slot.");
        }

        var registration = new GroupSlotRegistration
        {
            GroupId = groupId,
            SlotId = slotId,
            RegisteredBy = userId,
            RegisteredAt = now,
            Status = RegistrationStatus.Registered
        };

        await _context.GroupSlotRegistrations.AddAsync(registration);

        var newCount = activeCount + 1;
        slot.Status = newCount >= slot.MaxGroups ? SlotStatus.Full : SlotStatus.Open;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return registration;
    }

    public async Task Cancel(int registrationId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var registration = await _context.GroupSlotRegistrations.FirstOrDefaultAsync(x => x.RegistrationId == registrationId)
            ?? throw new KeyNotFoundException("Registration not found.");

        if (registration.Status == RegistrationStatus.Cancelled)
        {
            return;
        }

        var slot = await _context.Slots.FirstAsync(x => x.SlotId == registration.SlotId);
        var round = await _context.ReviewRounds.FirstAsync(x => x.RoundId == slot.RoundId);

        if (round.Status is ReviewRoundStatus.Closed or ReviewRoundStatus.Completed || slot.Status == SlotStatus.Locked)
        {
            throw new InvalidOperationException("Cannot cancel registration at this time.");
        }

        registration.Status = RegistrationStatus.Cancelled;
        if (slot.Status == SlotStatus.Full)
        {
            slot.Status = SlotStatus.Open;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}
