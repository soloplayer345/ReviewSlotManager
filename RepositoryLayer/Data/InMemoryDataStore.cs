using RepositoryLayer.Entities;
using RepositoryLayer.Enums;

namespace RepositoryLayer.Data;

public class InMemoryDataStore
{
    private readonly object _sync = new();

    public object SyncRoot => _sync;

    public List<User> Users { get; } =
    [
        new() { UserId = 1, FullName = "Moderator One", Email = "mod@rsm.local", Role = UserRole.Moderator },
        new() { UserId = 2, FullName = "GVHD One", Email = "gvhd1@rsm.local", Role = UserRole.Gvhd },
        new() { UserId = 3, FullName = "GV Review One", Email = "gvr1@rsm.local", Role = UserRole.GvReview },
        new() { UserId = 4, FullName = "Student One", Email = "sv1@rsm.local", Role = UserRole.Student },
        new() { UserId = 5, FullName = "Student Two", Email = "sv2@rsm.local", Role = UserRole.Student }
    ];

    public List<Semester> Semesters { get; } =
    [
        new()
        {
            SemesterId = 1,
            SemesterName = "HK1 2025-2026",
            StartDate = DateTime.UtcNow.AddMonths(-2),
            EndDate = DateTime.UtcNow.AddMonths(3),
            IsActive = true
        }
    ];

    public List<ReviewRound> ReviewRounds { get; } =
    [
        new()
        {
            RoundId = 1,
            SemesterId = 1,
            RoundNumber = 1,
            RoundName = "Review 1",
            RegistrationOpenAt = DateTime.UtcNow.AddDays(-2),
            RegistrationCloseAt = DateTime.UtcNow.AddDays(3),
            ReviewDateFrom = DateTime.UtcNow.AddDays(7),
            ReviewDateTo = DateTime.UtcNow.AddDays(10),
            Status = ReviewRoundStatus.Open
        }
    ];

    public List<Group> Groups { get; } =
    [
        new() { GroupId = 1, GroupName = "Group A", ProjectTitle = "AI Tutor", SemesterId = 1, GvhdId = 2 },
        new() { GroupId = 2, GroupName = "Group B", ProjectTitle = "Smart Slot", SemesterId = 1, GvhdId = 2 }
    ];

    public List<GroupMember> GroupMembers { get; } =
    [
        new() { MemberId = 1, GroupId = 1, StudentId = 4 },
        new() { MemberId = 2, GroupId = 2, StudentId = 5 }
    ];

    public List<ReviewerSlotConfig> ReviewerSlotConfigs { get; } =
    [
        new() { ConfigId = 1, RoundId = 1, MinSlots = 1, MaxSlots = 3, UpdatedBy = 1 }
    ];

    public List<Slot> Slots { get; } =
    [
        new()
        {
            SlotId = 1,
            RoundId = 1,
            StartTime = DateTime.UtcNow.AddDays(7).Date.AddHours(7),
            EndTime = DateTime.UtcNow.AddDays(7).Date.AddHours(9),
            Room = "A101",
            MaxGroups = 3,
            MinReviewers = 1,
            MaxReviewers = 3,
            Status = SlotStatus.Open,
            CreatedBy = 1
        },
        new()
        {
            SlotId = 2,
            RoundId = 1,
            StartTime = DateTime.UtcNow.AddDays(8).Date.AddHours(7),
            EndTime = DateTime.UtcNow.AddDays(8).Date.AddHours(9),
            Room = "A102",
            MaxGroups = 3,
            MinReviewers = 1,
            MaxReviewers = 3,
            Status = SlotStatus.Open,
            CreatedBy = 1
        }
    ];

    public List<GroupSlotRegistration> GroupSlotRegistrations { get; } = [];
    public List<ReviewerSlotRegistration> ReviewerSlotRegistrations { get; } = [];
    public List<Notification> Notifications { get; } = [];

    public int NextId<T>(IEnumerable<T> source, Func<T, int> idSelector)
    {
        var current = source.Select(idSelector).DefaultIfEmpty(0).Max();
        return current + 1;
    }
}
