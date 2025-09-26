using HackathonBot.Models;
using Microsoft.EntityFrameworkCore;
using MyBots.Core.Persistence;
using MyBots.Core.Persistence.DTO;

namespace HackathonBot;

internal class BotDbContext(DbContextOptions<BotDbContext> options) : BasicBotDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //
        // Participant
        //
        modelBuilder.Entity<Participant>(b =>
        {
            b.HasKey(p => p.Id);

            b.Property(p => p.Nickname)
             .IsRequired()
             .HasMaxLength(100);

            b.Property(p => p.FullName)
             .HasMaxLength(200);

            // Team relation: many participants -> one team (optional)
            b.HasOne(p => p.Team)
             .WithMany(t => t.Members)
             .HasForeignKey(p => p.TeamId)
             .OnDelete(DeleteBehavior.SetNull);

            // FK to FSM User by TelegramId (alternate/principal key)
            // Participant.TelegramId (nullable long) -> User.TelegramId (principal key)
            b.HasOne<User>()
             .WithMany()
             .HasForeignKey(p => p.TelegramId)
             .HasPrincipalKey(u => u.TelegramId)
             .OnDelete(DeleteBehavior.SetNull);

            // Index on TelegramId for fast lookup (nullable)
            b.HasIndex(p => p.TelegramId).HasDatabaseName("IX_Participant_TelegramId");
        });

        //
        // Team
        //
        modelBuilder.Entity<Team>(b =>
        {
            b.HasKey(t => t.Id);

            b.Property(t => t.Name)
             .IsRequired()
             .HasMaxLength(200);

            // Case enum stored as int
            b.Property(t => t.Case)
             .HasConversion<int>();

            b.Property(t => t.KmmId)
             .IsRequired();

            // Members navigation configured by Participant side
            // Optional: unique constraint on Team.Name per hackathon (single hackathon -> globally unique)
            b.HasIndex(t => t.Name).IsUnique();
        });

        //
        // Submission
        //
        modelBuilder.Entity<Submission>(b =>
        {
            b.HasKey(s => s.Id);

            b.Property(s => s.RepoUrl)
             .HasMaxLength(2000);

            b.Property(s => s.PresentationFileUrl)
             .HasMaxLength(2000);

            b.Property(s => s.PresentationLink)
             .HasMaxLength(2000);

            // Case enum stored as int
            b.Property(s => s.Case)
             .HasConversion<int>();

            // one-to-one Team <-> Submission (optional)
            b.HasOne(s => s.Team)
             .WithOne(t => t.Submission)
             .HasForeignKey<Submission>(s => s.TeamId)
             .OnDelete(DeleteBehavior.Cascade);

            // SubmittedBy -> Participant (FK)
            b.HasOne(s => s.SubmittedBy)
             .WithMany() // if you add Participant.Submissions collection, change here
             .HasForeignKey(s => s.SubmittedById)
             .OnDelete(DeleteBehavior.Restrict);

            // Only one submission per team
            b.HasIndex(s => s.TeamId).IsUnique();
        });

        //
        // BotUserRole (admins/organizers)
        //
        modelBuilder.Entity<BotUserRole>(b =>
        {
            b.HasKey(x => x.Id);

            // Username must be unique (you requested)
            b.Property(x => x.Username)
             .IsRequired()
             .HasMaxLength(200);
            b.HasIndex(x => x.Username)
             .IsUnique()
             .HasDatabaseName("IX_BotUserRole_Username");

            // TelegramId nullable: filled when user writes to bot
            b.Property(x => x.TelegramId);

            // Ensure we can map TelegramId -> FSM.User.TelegramId (principal key)
            b.HasIndex(x => x.TelegramId).HasDatabaseName("IX_BotUserRole_TelegramId");

            b.Property(x => x.Note)
             .HasMaxLength(2000);

            // Navigation to FSM User via alternate principal key User.TelegramId
            b.HasOne(x => x.User)
             .WithMany()
             .HasForeignKey(x => x.TelegramId)
             .HasPrincipalKey(u => u.TelegramId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        //
        // Enum mapping for RoleIndex (optional storage as int)
        //
        modelBuilder.Entity<BotUserRole>()
            .Property(b => b.RoleId)
            .HasConversion<int>()
            .HasDefaultValue(RoleIndex.Unknown);
    }
}