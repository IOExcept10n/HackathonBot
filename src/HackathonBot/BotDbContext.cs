using HackathonBot.Models;
using HackathonBot.Models.Kmm;
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

            b.HasOne(t => t.KmmTeam)
             .WithOne(k => k.HackathonTeam)
             .HasForeignKey<Team>(t => t.KmmId)
             .OnDelete(DeleteBehavior.Cascade);
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

        // Event <-> Quest (one-to-many)
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Quests)
            .WithOne(q => q.Event)
            .HasForeignKey(q => q.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // KmmTeam <-> EventEntry (one-to-many)
        modelBuilder.Entity<KmmTeam>()
            .HasMany(t => t.EventEntries)
            .WithOne(en => en.Team)
            .HasForeignKey(en => en.KmmTeamId)
            .OnDelete(DeleteBehavior.Cascade);

        // KmmTeam <-> AbilityUse (one-to-many)
        modelBuilder.Entity<KmmTeam>()
            .HasMany(t => t.AbilitiesLog)
            .WithOne(a => a.Team)
            .HasForeignKey(a => a.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        // KmmTeam <-> Team (one-to-one)
        modelBuilder.Entity<KmmTeam>()
            .Property(k => k.HackathonTeamId)
            .IsRequired();
        modelBuilder.Entity<KmmTeam>()
            .HasOne(k => k.HackathonTeam)
            .WithOne(t => t.KmmTeam)
            .HasForeignKey<KmmTeam>(t => t.HackathonTeamId)
            .OnDelete(DeleteBehavior.SetNull);

        // EventEntry: optional Quest relation (if QuestId can be 0/null adjust accordingly)
        modelBuilder.Entity<EventEntry>()
            .HasOne(en => en.Quest)
            .WithMany()
            .HasForeignKey(en => en.QuestId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes and constraints
        modelBuilder.Entity<Quest>()
            .HasIndex(q => q.EventId);

        modelBuilder.Entity<EventEntry>()
            .HasIndex(e => new { e.KmmTeamId, e.QuestId });

        modelBuilder.Entity<AbilityUse>()
            .HasIndex(a => new { a.TeamId, a.Ability, a.UsedAt });

        // Enum conversions (optional): store enums as strings for readability
        modelBuilder.Entity<KmmTeam>()
            .Property(t => t.Role)
            .HasConversion<string>();

        modelBuilder.Entity<AbilityUse>()
            .Property(a => a.Ability)
            .HasConversion<string>();

        // Configure required fields and max lengths
        modelBuilder.Entity<Event>()
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<Quest>()
            .Property(q => q.Name)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<Bank>()
            .Property(b => b.Key)
            .IsRequired()
            .HasMaxLength(200);
    }
}
