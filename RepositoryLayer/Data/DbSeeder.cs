using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;

namespace RepositoryLayer.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ReviewSlotDbContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var now = DateTime.UtcNow;
        var defaultPassword = BCrypt.Net.BCrypt.HashPassword("Password@123");

        var moderator = new User { FullName = "Nguyen Van Mod", Email = "modnv@fpt.edu.vn", PasswordHash = defaultPassword, Role = UserRole.Moderator, CreatedAt = now, UpdatedAt = now };
        var gvhd = new User { FullName = "Tran Van Giang", Email = "giangtv@fpt.edu.vn", PasswordHash = defaultPassword, Role = UserRole.Gvhd, CreatedAt = now, UpdatedAt = now };
        var studentOne = new User { FullName = "Le Hoang Nam", Email = "namlhse170001@fpt.edu.vn", PasswordHash = defaultPassword, Role = UserRole.Student, CreatedAt = now, UpdatedAt = now };
        var studentTwo = new User { FullName = "Pham Minh Duc", Email = "ducpmse170002@fpt.edu.vn", PasswordHash = defaultPassword, Role = UserRole.Student, CreatedAt = now, UpdatedAt = now };
        var studentThree = new User { FullName = "Vo Thanh Tung", Email = "tungvtse170003@fpt.edu.vn", PasswordHash = defaultPassword, Role = UserRole.Student, CreatedAt = now, UpdatedAt = now };

        await context.Users.AddRangeAsync(moderator, gvhd, studentOne, studentTwo, studentThree);
        await context.SaveChangesAsync();

        var semester = new Semester
        {
            SemesterName = "HK1 2025-2026",
            StartDate = now.AddMonths(-2),
            EndDate = now.AddMonths(3),
            IsActive = true,
            CreatedAt = now
        };
        await context.Semesters.AddAsync(semester);
        await context.SaveChangesAsync();

        var round = new ReviewRound
        {
            SemesterId = semester.SemesterId,
            RoundNumber = 1,
            RoundName = "Review 1",
            RegistrationOpenAt = now.AddDays(-2),
            RegistrationCloseAt = now.AddDays(3),
            ReviewDateFrom = now.AddDays(7),
            ReviewDateTo = now.AddDays(10),
            Status = ReviewRoundStatus.Open
        };
        await context.ReviewRounds.AddAsync(round);
        await context.SaveChangesAsync();

        var groupA = new Group { GroupName = "Group A", ProjectTitle = "AI Tutor", SemesterId = semester.SemesterId, GvhdId = gvhd.UserId, CreatedAt = now, UpdatedAt = now };
        var groupB = new Group { GroupName = "Group B", ProjectTitle = "Smart Slot", SemesterId = semester.SemesterId, GvhdId = gvhd.UserId, CreatedAt = now, UpdatedAt = now };
        await context.Groups.AddRangeAsync(groupA, groupB);
        await context.SaveChangesAsync();

        var memberA = new GroupMember { GroupId = groupA.GroupId, StudentId = studentOne.UserId, JoinedAt = now };
        var memberB = new GroupMember { GroupId = groupB.GroupId, StudentId = studentTwo.UserId, JoinedAt = now };

        var config = new ReviewerSlotConfig { RoundId = round.RoundId, MinSlots = 1, MaxSlots = 3, UpdatedBy = moderator.UserId };

        var slotOne = new Slot
        {
            RoundId = round.RoundId,
            StartTime = now.AddDays(7).Date.AddHours(7),
            EndTime = now.AddDays(7).Date.AddHours(9),
            Room = "A101",
            MaxGroups = 3,
            MinReviewers = 1,
            MaxReviewers = 3,
            Status = SlotStatus.Open,
            CreatedBy = moderator.UserId
        };
        var slotTwo = new Slot
        {
            RoundId = round.RoundId,
            StartTime = now.AddDays(8).Date.AddHours(7),
            EndTime = now.AddDays(8).Date.AddHours(9),
            Room = "A102",
            MaxGroups = 3,
            MinReviewers = 1,
            MaxReviewers = 3,
            Status = SlotStatus.Open,
            CreatedBy = moderator.UserId
        };

        await context.GroupMembers.AddRangeAsync(memberA, memberB);
        await context.ReviewerSlotConfigs.AddAsync(config);
        await context.Slots.AddRangeAsync(slotOne, slotTwo);
        await context.SaveChangesAsync();
    }
}
