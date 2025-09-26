using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MyBots.Core.Fsm.States;
using MyBots.Core.Persistence.DTO;
using MyBots.Modules.Common.Handling;
using MyBots.Modules.Common.Interactivity;

namespace MyBots.Modules.Common.Roles
{
    public class RoleDispatcher : IStateHandlerProvider, IUserStateService
    {
        private readonly IHandlerRegistrationService _handlerRegistration;
        private readonly IRoleProvider _roleProvider;
        private readonly IStateHandlerRegistry _handlers;
        private readonly IReadOnlyList<ModuleBase> _modules;
        private readonly Dictionary<string, ModuleBase> _moduleMap;

        private readonly Dictionary<Role, StateDefinition> _registeredRoles = [];

        public RoleDispatcher(IServiceProvider serviceProvider)
        {
            _handlerRegistration = serviceProvider.GetRequiredService<IHandlerRegistrationService>();
            _roleProvider = serviceProvider.GetRequiredService<IRoleProvider>();
            _handlers = serviceProvider.GetRequiredService<IStateHandlerRegistry>();
            _modules = [..serviceProvider.GetServices<ModuleBase>()];
            _moduleMap = _modules.ToDictionary(x => x.GetType().Name, y => y);
            foreach (var module in _modules)
            {
                _handlerRegistration.RegisterModule(module, _handlers);
            }
        }

        public async Task<StateDefinition> GetUserRootStateAsync(User user) => _registeredRoles[await _roleProvider.GetRoleAsync(user)];

        public void RegisterRole(Role role, string helloMessage, string? accessDeniedMessage)
        {
            var allowedModules = _modules.Where(module => module.AllowedRoles.Contains(role));

            var layout = new MenuStateLayout()
            {
                MessageText = helloMessage,
                Buttons = allowedModules.Select(x => new List<ButtonLabel>() { x.Label })
            };

            var definition = new StateDefinition(role.Name, "start", string.Empty, layout);
            _registeredRoles[role] = definition;

            var handler = new RoleStateHandler(definition, [..allowedModules], accessDeniedMessage);
            _handlers.Register(definition, handler);
        }

        public bool TryGetHandler(StateDefinition state, [NotNullWhen(true)] out IStateHandler? handler)
        {
            if (state.Module != null && _moduleMap.TryGetValue(state.Module, out var module) && !module.IsEnabled)
            {
                handler = null;
                return false;
            }

            return _handlers.TryGet(state, out handler);
        }

        private class RoleStateHandler(
            StateDefinition selfDefinition,
            IEnumerable<ModuleBase> allowedModules,
            string? accessDeniedMessage) : IStateHandler
        {
            public Task<StateResult> ExecuteAsync(StateContext ctx, CancellationToken cancellationToken)
            {
                var selectedModule = allowedModules.FirstOrDefault(x => ctx.Matches(x.Label));
                if (selectedModule == null)
                {
                    return Task.FromResult(new StateResult(selfDefinition.StateId, string.Empty, accessDeniedMessage));
                }

                return Task.FromResult(new StateResult(selectedModule.RootStateId, string.Empty));
            }
        }
    }
}