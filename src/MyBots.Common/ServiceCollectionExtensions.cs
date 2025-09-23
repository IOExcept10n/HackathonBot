using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyBots.Core.Fsm;
using MyBots.Core.Localization;
using MyBots.Persistence.DTO;
using MyBots.Persistence.Repositories;
using MyBots.Persistence.Repository;

namespace MyBots.Core;

/// <summary>
/// Provides extension methods for registering bot framework services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds core bot framework services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddBotFramework(this IServiceCollection services)
    {
        // FSM services
        services.AddSingleton<IStateRegistry, StateRegistry>();
        services.AddScoped<IFsmDispatcher, FsmDispatcher>();
        services.AddSingleton<IKeyboardBuilder, KeyboardBuilder>();
        services.AddScoped<IPromptService, PromptService>();

        return services;
    }

    /// <summary>
    /// Adds localization services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configure">Optional action to configure localization options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddLocalization(
        this IServiceCollection services,
        Action<LocalizationOptions>? configure = null)
    {
        services.AddOptions<LocalizationOptions>();
        if (configure != null)
        {
            services.Configure(configure);
        }

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<LocalizationOptions>>().Value;
            return new JsonResourceManager(options.ResourceBaseName, options.ResourcesPath);
        });

        services.AddScoped<ILocalizationService>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<LocalizationOptions>>().Value;
            var resourceManager = sp.GetRequiredService<JsonResourceManager>();
            return new LocalizationService(resourceManager, options.DefaultLanguage);
        });

        return services;
    }

    /// <summary>
    /// Adds persistence services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddRolesPersistence(this IServiceCollection services)
    {
        services.AddScoped<IRepository<User>, UserRepository>();
        services.AddScoped<IRepository<Role>, RoleRepository>();
        services.AddScoped<IRepository<AuditLog>, AuditLogRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<RoleRepository>();
        services.AddScoped<AuditLogRepository>();

        return services;
    }
}