using System.Reflection;
using HackathonBot.Models;
using HackathonBot.Models.Kmm;
using HackathonBot.Modules;
using HackathonBot.Properties;
using HackathonBot.Repository;
using HackathonBot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyBots.Core;
using MyBots.Core.Persistence;
using MyBots.Core.Persistence.DTO;
using MyBots.Core.Persistence.Repository;
using MyBots.Modules.Common;
using MyBots.Modules.Common.Roles;

namespace HackathonBot;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHackathonBot(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configuration
        services.Configure<BotStartupConfig>(configuration.GetSection(nameof(BotStartupConfig)));
        services.Configure<HackathonConfig>(configuration.GetSection(nameof(HackathonConfig)));
        services.Configure<KmmConfig>(configuration.GetSection(nameof(KmmConfig)));
        services.AddLogging();

        // Database
        services.AddDbContext<BotDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("Sqlite")
                ?? throw new InvalidOperationException("Database connection string is not configured")));
        services.AddScoped<BasicBotDbContext>(ctx => ctx.GetRequiredService<BotDbContext>());
        services.AddRepositories();
        services.AddKmmRepositories();

        // Common Services
        services.ConfigureCommonServices();

        foreach (var moduleType in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAssignableTo(typeof(ModuleBase)) && !x.IsAbstract))
        {
            services.AddSingleton(typeof(ModuleBase), moduleType);
        }

        services.ConfigureRoleDispatcher(
            new(Role.Unknown, Localization.UnknownRoleHello, Localization.UnknownRoleAccessDenied),
            new(Roles.Participant, Localization.ParticipantHello, Localization.ParticipantAccessDenied),
            new(Roles.Organizer, Localization.OrganizerHello, Localization.OrganizerAccessDenied),
            new(Roles.Admin, Localization.AdminHello, Localization.OrganizerAccessDenied));
        
        services.AddSingleton<IRoleProvider, RoleProvider>();
        services.AddSingleton<ITelegramUserService, TelegramUserService>();
        services.AddSingleton<IKmmGameService, KmmGameService>();

        services.AddSingleton(Localization.ResourceManager);

        return services;
    }

    public static IServiceCollection AddKmmRepositories(this IServiceCollection services)
    {
        // Bank
        services.AddScoped<BankRepository>();
        services.AddScoped<IBankRepository>(sp => sp.GetRequiredService<BankRepository>());
        services.AddScoped<IRepository<Bank>>(sp => sp.GetRequiredService<BankRepository>());

        // KmmTeam
        services.AddScoped<KmmTeamRepository>();
        services.AddScoped<IKmmTeamRepository>(sp => sp.GetRequiredService<KmmTeamRepository>());
        services.AddScoped<IRepository<KmmTeam>>(sp => sp.GetRequiredService<KmmTeamRepository>());

        // AbilityUse
        services.AddScoped<AbilityUseRepository>();
        services.AddScoped<IAbilityUseRepository>(sp => sp.GetRequiredService<AbilityUseRepository>());
        services.AddScoped<IRepository<AbilityUse>>(sp => sp.GetRequiredService<AbilityUseRepository>());

        // Quest
        services.AddScoped<QuestRepository>();
        services.AddScoped<IQuestRepository>(sp => sp.GetRequiredService<QuestRepository>());
        services.AddScoped<IRepository<Quest>>(sp => sp.GetRequiredService<QuestRepository>());

        // EventEntry
        services.AddScoped<EventEntryRepository>();
        services.AddScoped<IEventEntryRepository>(sp => sp.GetRequiredService<EventEntryRepository>());
        services.AddScoped<IRepository<EventEntry>>(sp => sp.GetRequiredService<EventEntryRepository>());

        // Event
        services.AddScoped<EventRepository>();
        services.AddScoped<IEventRepository>(sp => sp.GetRequiredService<EventRepository>());
        services.AddScoped<IRepository<Event>>(sp => sp.GetRequiredService<EventRepository>());

        // EventAuditEntry
        services.AddScoped<EventAuditRepository>();
        services.AddScoped<IEventAuditRepository>(sp => sp.GetRequiredService<EventAuditRepository>());
        services.AddScoped<IRepository<EventAuditEntry>>(sp => sp.GetRequiredService<EventAuditRepository>());

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // FSM User
        services.AddScoped<UserRepository>();
        services.AddScoped<IUserRepository>(ctx => ctx.GetRequiredService<UserRepository>());
        services.AddScoped<IRepository<User>>(ctx => ctx.GetRequiredService<UserRepository>());

        // Participant
        services.AddScoped<ParticipantRepository>();
        services.AddScoped<IParticipantRepository>(sp => sp.GetRequiredService<ParticipantRepository>());
        services.AddScoped<IRepository<Participant>>(sp => sp.GetRequiredService<ParticipantRepository>());

        // Team
        services.AddScoped<TeamRepository>();
        services.AddScoped<ITeamRepository>(sp => sp.GetRequiredService<TeamRepository>());
        services.AddScoped<IRepository<Team>>(sp => sp.GetRequiredService<TeamRepository>());

        // Submission
        services.AddScoped<SubmissionRepository>();
        services.AddScoped<ISubmissionRepository>(sp => sp.GetRequiredService<SubmissionRepository>());
        services.AddScoped<IRepository<Submission>>(sp => sp.GetRequiredService<SubmissionRepository>());

        // BotUserRole
        services.AddScoped<BotUserRoleRepository>();
        services.AddScoped<IBotUserRoleRepository>(sp => sp.GetRequiredService<BotUserRoleRepository>());
        services.AddScoped<IRepository<BotUserRole>>(sp => sp.GetRequiredService<BotUserRoleRepository>());

        return services;
    }

    public static async Task ConfigureCreatorAsync(this IServiceProvider services)
    {
        var startup = services.GetRequiredService<IOptions<BotStartupConfig>>().Value;
        if (startup.BotCreator == null)
            return;
        var roles = services.GetRequiredService<IBotUserRoleRepository>();
        var role = await roles.FindByUsernameAsync(startup.BotCreator);
        if (role == null)
        {
            role = new()
            {
                Note = "Creator",
                RoleId = RoleIndex.Admin,
                Username = startup.BotCreator
            };
            await roles.AddAsync(role);
            await roles.SaveChangesAsync();
        }
    }

    public static void ConfigureMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BotDbContext>();
        db.Database.Migrate();
    }
}