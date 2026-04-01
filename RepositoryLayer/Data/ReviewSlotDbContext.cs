using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Data;

public class ReviewSlotDbContext : DbContext
{
    public ReviewSlotDbContext(DbContextOptions<ReviewSlotDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Semester> Semesters => Set<Semester>();
    public DbSet<ReviewRound> ReviewRounds => Set<ReviewRound>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
    public DbSet<ReviewerSlotConfig> ReviewerSlotConfigs => Set<ReviewerSlotConfig>();
    public DbSet<Slot> Slots => Set<Slot>();
    public DbSet<GroupSlotRegistration> GroupSlotRegistrations => Set<GroupSlotRegistration>();
    public DbSet<ReviewerSlotRegistration> ReviewerSlotRegistrations => Set<ReviewerSlotRegistration>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasKey(x => x.UserId);
        modelBuilder.Entity<Semester>().HasKey(x => x.SemesterId);
        modelBuilder.Entity<ReviewRound>().HasKey(x => x.RoundId);
        modelBuilder.Entity<Group>().HasKey(x => x.GroupId);
        modelBuilder.Entity<GroupMember>().HasKey(x => x.MemberId);
        modelBuilder.Entity<ReviewerSlotConfig>().HasKey(x => x.ConfigId);
        modelBuilder.Entity<Slot>().HasKey(x => x.SlotId);
        modelBuilder.Entity<GroupSlotRegistration>().HasKey(x => x.RegistrationId);
        modelBuilder.Entity<ReviewerSlotRegistration>().HasKey(x => x.ReviewerRegistrationId);
        modelBuilder.Entity<Notification>().HasKey(x => x.NotificationId);

        modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(255);
        modelBuilder.Entity<ReviewerSlotConfig>().Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<ReviewRound>()
            .HasOne<Semester>()
            .WithMany()
            .HasForeignKey(x => x.SemesterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Group>()
            .HasOne<Semester>()
            .WithMany()
            .HasForeignKey(x => x.SemesterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Group>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.GvhdId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupMember>()
            .HasOne<Group>()
            .WithMany()
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupMember>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReviewerSlotConfig>()
            .HasOne<ReviewRound>()
            .WithMany()
            .HasForeignKey(x => x.RoundId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ReviewerSlotConfig>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Slot>()
            .HasOne<ReviewRound>()
            .WithMany()
            .HasForeignKey(x => x.RoundId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Slot>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupSlotRegistration>()
            .HasOne<Group>()
            .WithMany()
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupSlotRegistration>()
            .HasOne<Slot>()
            .WithMany()
            .HasForeignKey(x => x.SlotId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupSlotRegistration>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.RegisteredBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReviewerSlotRegistration>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReviewerSlotRegistration>()
            .HasOne<Slot>()
            .WithMany()
            .HasForeignKey(x => x.SlotId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Notification>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
