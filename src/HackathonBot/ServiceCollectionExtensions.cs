using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBots.Core.Configuration;
using MyBots.Core.FSM;
using MyBots.Core.Localization;
using MyBots.Core.Modules;
using MyBots.Core.Repository;
using MyBots.Core.Scheduling;
using Quartz;
using Telegram.Bot;

namespace HackathonBot;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHackathonBot(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configuration
        services.Configure<BotConfiguration>(configuration.GetSection("Bot"));
        services.Configure<DatabaseConfiguration>(configuration.GetSection("Database"));
        services.Configure<ModulesConfiguration>(configuration.GetSection("Modules"));
        services.Configure<LocalizationConfiguration>(configuration.GetSection("Localization"));

        // Database
        services.AddDbContext<BotDbContext>(options =>
            options.UseSqlite(
                configuration.GetSection("Database").Get<DatabaseConfiguration>()?.ConnectionString
                ?? throw new InvalidOperationException("Database connection string is not configured")));

        // Bot Client
        var token = configuration.GetSection("Bot").Get<BotConfiguration>()?.Token
            ?? throw new InvalidOperationException("Bot token is not configured");
        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(token));

        // Common Services
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<IFiniteStateMachine, FiniteStateMachine>();
        services.AddSingleton<IModuleManager, ModuleManager>();
        services.AddSingleton<ILocalizationService, LocalizationService>();

        // Scheduling
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("SendMessageJob");
            q.AddJob<MyBots.Core.Scheduling.Jobs.SendMessageJob>(opts => opts.WithIdentity(jobKey));
            
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("SendMessageJob-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever()));
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        services.AddSingleton<IScheduleManager, QuartzScheduleManager>();

        // Modules (будут добавлены позже)
        // services.AddScoped<IModule, AdminModule>();
        // services.AddScoped<IModule, OrganizerModule>();
        // services.AddScoped<IModule, ParticipantModule>();
        // services.AddScoped<IModule, ViewerModule>();

        return services;
    }
}