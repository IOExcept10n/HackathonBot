using System.Collections.Frozen;
using MyBots.Core.Fsm.States;
using MyBots.Core.Localization;
using MyBots.Modules.Common.Interactivity;
using MyBots.Modules.Common.Roles;

namespace MyBots.Modules.Common;

public abstract class ModuleBase(
    ButtonLabel label,
    IEnumerable<Role> allowedRoles,
    IStateRegistry states,
    ILocalizationService localization)
{
    public const string SelectRootMenuKey = "ModuleSelectRootMenu";
    public const string InvalidInputKey = "BaseModuleInvalidInput";
    public const string UnknownErrorKey = "UnknownError";
    public const string RootStateName = "root";

    private readonly IStateRegistry _states = states;
    private readonly ILocalizationService _localizationService = localization;

    private string? moduleName = null;

    public bool IsEnabled { get; set; } = true;

    public string Name => moduleName ??= GetType().Name;

    public string RootStateId => $"{Name}:root";

    public ButtonLabel Label { get; } = label;

    public IReadOnlySet<Role> AllowedRoles { get; } = FrozenSet.ToFrozenSet(allowedRoles);

    [MenuState(SelectRootMenuKey, StateName = RootStateName)]
    public abstract Task<StateResult> OnModuleRootAsync(ModuleStateContext ctx);

    public virtual ModuleStateContext PrepareContext(RoleStateContext ctx, CancellationToken cancellationToken) =>
        new(ctx.User, ctx.User.TelegramId, ctx.Role, ctx.Message, ctx.StateData, ctx.BotClient, cancellationToken);

    protected static StateResult Retry(ModuleStateContext ctx, string? stateData = null, string? message = null)
        => new(ctx.User.State, stateData ?? ctx.User.StateData, message);

    protected static StateResult ToStart(string? stateData = null, string? message = null)
        => new("start", stateData ?? string.Empty, message);

    protected StateResult Fail(string? stateData = null, string? message = null)
        => ToStart(stateData ?? _localizationService.GetString(UnknownErrorKey), message);

    protected internal virtual StateResult InvalidInput(ModuleStateContext ctx)
        => Retry(ctx, message: _localizationService.GetString(InvalidInputKey));
    protected StateResult ToRoot(string? stateData = null, string? message = null)
        => new(RootStateId, stateData ?? string.Empty, message);

    protected StateResult Completed(string message)
        => ToRoot(message: message);

    protected StateResult ToGlobalState<TModule>(string stateKey, string? stateData = null, string? message = null)
    {
        string stateId = $"{typeof(TModule).Name}:{stateKey}";
        if (!_states.TryGet(stateId, out _))
            throw new InvalidOperationException("Cannot move to the state that doesn't exist");
        return new(stateId, stateData ?? string.Empty, message);
    }

    protected internal StateResult Back(ModuleStateContext ctx, string? stateData = null, string? message = null)
    {
        if (ctx.User.State == RootStateId)
            return ToStart(stateData, message);
        if (_states.TryGet(ctx.User.State, out var current))
            return new(current.ParentStateId, stateData ?? string.Empty, message);
        return ToRoot(stateData, message);
    }

    protected StateResult ToState(string stateKey, string? stateData = null, string? message = null)
    {
        string stateId = $"{Name}:{stateKey}";
        if (!_states.TryGet(stateId, out _))
            throw new InvalidOperationException("Cannot move to the state that doesn't exist");
        return new(stateId, stateData ?? string.Empty, message);
    }
}
