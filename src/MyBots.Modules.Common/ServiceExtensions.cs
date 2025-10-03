using Microsoft.Extensions.DependencyInjection;
using MyBots.Core.Fsm;
using MyBots.Core.Fsm.States;
using MyBots.Core.Localization;
using MyBots.Modules.Common.Handling;
using MyBots.Modules.Common.Interactivity;
using MyBots.Modules.Common.Roles;

namespace MyBots.Modules.Common;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureRoleDispatcher(this IServiceCollection services, params RoleConfigurationEntry[] roles)
    {
        services.AddSingleton(ctx =>
        {
            var dispatcher = new RoleDispatcher(ctx);
            foreach (var role in roles)
                dispatcher.RegisterRole(role.Role, role.HelloMessage, role.AccessDeniedMessage);
            return dispatcher;
        });
        services.AddSingleton<IUserStateService>(ctx => ctx.GetRequiredService<RoleDispatcher>());
        services.AddSingleton<IStateHandlerProvider>(ctx => ctx.GetRequiredService<RoleDispatcher>());
        return services;
    }

    public static IServiceCollection ConfigureCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IFsmDispatcher, FsmDispatcher>();
        services.AddSingleton<ILocalizationService, LocalizationService>();
        services.AddSingleton<IStateRegistry, StateRegistry>();
        services.AddSingleton<IHandlerRegistrationService, HandlerRegistrationService>();
        services.AddSingleton<IStateHandlerRegistry, StateHandlerRegistry>();
        services.AddSingleton<IReplyService, ReplyService>();
        services.AddSingleton<IButtonLabelProvider, ButtonLabelProvider>();
        services.AddSingleton<IModelRegistry, ModelRegistry>();

        return services;
    }
}

public readonly record struct RoleConfigurationEntry(Role Role, string HelloMessage, string? AccessDeniedMessage);
