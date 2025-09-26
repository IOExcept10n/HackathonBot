using System.Reflection;
using System.Resources;
using HackathonBot.Models;
using HackathonBot.Properties;
using HackathonBot.Repository;
using HackathonBot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBots.Core;
using MyBots.Core.Fsm;
using MyBots.Core.Fsm.States;
using MyBots.Core.Localization;
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
        services.Configure<BotStartupConfig>(configuration.GetSection("StartupConfig"));
        services.AddLogging();

        // Database
        services.AddDbContext<BotDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("Sqlite")
                ?? throw new InvalidOperationException("Database connection string is not configured")));
        services.AddScoped<BasicBotDbContext>(ctx => ctx.GetRequiredService<BotDbContext>());
        services.AddRepositories();

        // Common Services
        services.ConfigureCommonServices();

        foreach (var moduleType in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAssignableTo(typeof(ModuleBase))))
        {
            services.AddSingleton(typeof(ModuleBase), moduleType);
        }

        services.ConfigureRoleDispatcher(
            new(Role.Unknown, Localization.UnknownRoleHello, Localization.UnknownRoleAccessDenied),
            new(Roles.Participant, "Hello, user!", Localization.ParticipantAccessDenied),
            new(Roles.Organizer, "Welcome, master ^_^", Localization.OrganizerAccessDenied),
            new(Roles.Admin, "Hi adm :3", Localization.OrganizerAccessDenied));
        
        services.AddSingleton<IRoleProvider, RoleProvider>();
        services.AddSingleton<ITelegramUserService, TelegramUserService>();

        services.AddSingleton(Localization.ResourceManager);

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
}