using System.Reflection;
using MyBots.Core.Fsm.States;
using MyBots.Core.Localization;
using MyBots.Modules.Common.Interactivity;
using MyBots.Modules.Common.Roles;

namespace MyBots.Modules.Common.Handling
{
    public class HandlerRegistrationService(IButtonLabelProvider buttonProvider, ILocalizationService localization, IRoleProvider roleProvider) : IHandlerRegistrationService
    {
        private readonly IButtonLabelProvider _buttonProvider = buttonProvider;
        private readonly ILocalizationService _localization = localization;
        private readonly IRoleProvider _roleProvider = roleProvider;

        public void RegisterModule(ModuleBase module, IStateHandlerRegistry registry)
        {
            var stateMethodCandidates = from method in module.GetType().GetMethods()
                                        let stateAttr = method.GetCustomAttribute<FsmStateAttribute>()
                                        where stateAttr != null
                                        select new
                                        {
                                            Method = method,
                                            State = stateAttr
                                        };

            foreach (var candidate in stateMethodCandidates)
            {
                StateDefinition def = candidate.State switch
                {
                    MenuStateAttribute menuState => RegisterMenu(registry, module, menuState, candidate.Method, candidate.Method.GetCustomAttributes<MenuItemAttribute>()),
                    PromptStateAttribute promptState => RegisterPrompt(registry, module, promptState, candidate.Method),
                    _ => throw new InvalidOperationException(
                        "Cannot register method because of unrecognizable state definition." +
                        "Write your oen implementation of the IHandlerRegistrationService to use custom state definitions."),
                };
            }
        }

        private StateDefinition RegisterPrompt(IStateHandlerRegistry registry, ModuleBase module, PromptStateAttribute promptAttr, MethodInfo method)
        {
            var handler = CreatePromptStateHandler(module, promptAttr, method);
            StateDefinition definition = CreatePromptDefinition(module, promptAttr, method);

            registry.Register(definition, handler);
            return definition;
        }

        private StateDefinition CreatePromptDefinition(ModuleBase module, PromptStateAttribute promptAttr, MethodInfo method)
        {
            var layout = new PromptStateLayout() { MessageText = _localization.GetString(promptAttr.MessageResourceKey) };

            return new StateDefinition(
                promptAttr.StateName ?? method.Name,
                module.Name,
                $"{module.Name}:{promptAttr.ParentStateName}",
                layout);
        }

        private IStateHandler CreatePromptStateHandler(ModuleBase module, PromptStateAttribute promptAttr, MethodInfo method)
            => (IStateHandler?)Activator.CreateInstance(
                typeof(PromptStateHandler<>).MakeGenericType(promptAttr.InputType), 
                [module, method, _roleProvider, promptAttr.AllowTextInput, promptAttr.AllowFileInput]) ??
            throw new InvalidOperationException("Couldn't create prompt state handler. Please, check your prompt handling method definition.");

        private StateDefinition RegisterMenu(IStateHandlerRegistry registry,
                                             ModuleBase module,
                                             MenuStateAttribute menuAttr,
                                             MethodInfo method,
                                             IEnumerable<MenuItemAttribute> menuItems)
        {
            var handler = new MenuStateHandler(module, method, _roleProvider);
            var definition = CreateMenuDefinition(module, menuAttr, method, menuItems);
            registry.Register(definition, handler);
            return definition;
        }

        private StateDefinition CreateMenuDefinition(ModuleBase module, MenuStateAttribute menuAttr, MethodInfo method, IEnumerable<MenuItemAttribute> menuItems)
        {
            var layout = new MenuStateLayout()
            {
                MessageText = _localization.GetString(menuAttr.MessageResourceKey),
                Buttons = [from item in menuItems select _buttonProvider.GetLabel(item.LabelKey)],
            };

            return new StateDefinition(
                menuAttr.StateName ?? method.Name,
                module.Name,
                // Special case for root module state. It should refer to the main menu as parent.
                menuAttr.StateName != ModuleBase.RootStateName ? $"{module.Name}:{menuAttr.ParentStateName}" : "start",
                layout);
        }

        private class MenuStateHandler : IStateHandler
        {
            private readonly Func<ModuleStateContext, Task<StateResult>> _expr;
            private readonly ModuleBase _module;
            private readonly IRoleProvider _roleProvider;

            public MenuStateHandler(ModuleBase module, MethodInfo method, IRoleProvider roleProvider)
            {
                _module = module;
                _expr = GetMenuStateExpression(method);
                _roleProvider = roleProvider;
            }

            public async Task<StateResult> ExecuteAsync(StateContext ctx, CancellationToken cancellationToken = default)
            {
                var role = await _roleProvider.GetRoleAsync(ctx.User.TelegramId, cancellationToken);
                var moduleContext = _module.PrepareContext(RoleStateContext.FromContext(ctx, role), cancellationToken);
                return await _expr(moduleContext);
            }

            private Func<ModuleStateContext, Task<StateResult>> GetMenuStateExpression(MethodInfo method)
                => method.CreateDelegate<Func<ModuleStateContext, Task<StateResult>>>(_module);
        }

        private class PromptStateHandler<T> : IStateHandler where T : IParsable<T>
        {
            private readonly Func<PromptStateContext<T>, Task<StateResult>> _expr;
            private readonly ModuleBase _module;
            private readonly IRoleProvider _roleProvider;
            private readonly bool _allowTextInput;
            private readonly bool _allowFileInput;

            public PromptStateHandler(ModuleBase module, MethodInfo method, IRoleProvider roleProvider, bool allowTextInput, bool allowFileInput)
            {
                _module = module;
                _expr = GetPromptStateExpression(method);
                _roleProvider = roleProvider;
                _allowTextInput = allowTextInput;
                _allowFileInput = allowFileInput;
            }

            public async Task<StateResult> ExecuteAsync(StateContext ctx, CancellationToken cancellationToken = default)
            {
                var role = await _roleProvider.GetRoleAsync(ctx.User.TelegramId, cancellationToken);
                var moduleContext = _module.PrepareContext(RoleStateContext.FromContext(ctx, role), cancellationToken);

                (Optional<T> data, bool invalidContent) = ctx.Message switch
                {
                    TextMessageContent text when _allowTextInput && T.TryParse(text.Text, null, out var d) => (d, false),
                    FileMessageContent when _allowFileInput => (Optional<T>.None, false),
                    _ => (Optional<T>.None, true),
                };

                if (invalidContent)
                    return _module.InvalidInput(moduleContext);

                var promptContext = new PromptStateContext<T>(
                    moduleContext.User,
                    moduleContext.Chat,
                    moduleContext.Role,
                    moduleContext.Message,
                    data,
                    moduleContext.StateData,
                    moduleContext.BotClient,
                    moduleContext.CancellationToken);

                return await _expr(promptContext);
            }

            private Func<PromptStateContext<T>, Task<StateResult>> GetPromptStateExpression(MethodInfo method)
                => method.CreateDelegate<Func<PromptStateContext<T>, Task<StateResult>>>(_module);
        }

        //private class ModuleRootStateHandler(ModuleBase module, IRoleProvider roleProvider) : IStateHandler
        //{
        //    private readonly ModuleBase _module = module;
        //    private readonly IRoleProvider _roleProvider = roleProvider;

        //    public async Task<StateResult> ExecuteAsync(StateContext ctx, CancellationToken cancellationToken = default)
        //    {
        //        var role = await _roleProvider.GetRoleAsync(ctx.User.TelegramId, cancellationToken);
        //        var roleContext = RoleStateContext.FromContext(ctx, role);
        //        var moduleContext = _module.PrepareContext(roleContext, cancellationToken);
        //        return await _module.OnModuleRootAsync(moduleContext);
        //    }
        //}
    }
}
