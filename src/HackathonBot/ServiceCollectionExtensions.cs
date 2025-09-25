using System.Reflection;
using System.Resources;
using HackathonBot.Properties;
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
using MyBots.Modules.Common.Handling;
using MyBots.Modules.Common.Interactivity;
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
        services.AddScoped<UserRepository>();
        services.AddScoped<IRepository<User>>(ctx => ctx.GetRequiredService<UserRepository>());

        // Common Services
        services.AddSingleton<IFsmDispatcher, FsmDispatcher>();
        services.AddSingleton<ILocalizationService, LocalizationService>();
        services.AddSingleton<IStateRegistry, StateRegistry>();
        services.AddSingleton<IRoleProvider, RoleProvider>();
        services.AddSingleton<IHandlerRegistrationService, HandlerRegistrationService>();
        services.AddSingleton<IStateHandlerRegistry, StateHandlerRegistry>();
        services.AddSingleton<IReplyService, ReplyService>();
        services.AddSingleton<IButtonLabelProvider, ButtonLabelProvider>();

        foreach (var moduleType in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAssignableTo(typeof(ModuleBase))))
        {
            services.AddSingleton(typeof(ModuleBase), moduleType);
        }

        services.ConfigureRoleDispatcher(
            new(Role.Unknown, "Idk u", "u shll nt pss"),
            new(Roles.User, "Hello, user!", "U cant"),
            new(Roles.Organizer, "Welcome, master ^_^", "baka!"),
            new(Roles.Admin, "Hi adm :3", "sorry, nope"));

        services.AddSingleton(Localization.ResourceManager);

        return services;
    }
}