using System.Collections.Frozen;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyBots.Core.Fsm.States;
using MyBots.Core.Localization;
using MyBots.Modules.Common.Interactivity;
using MyBots.Modules.Common.Roles;

namespace MyBots.Modules.Common;

public abstract class ModuleBase
{
    public const string SelectRootMenuKey = "ModuleSelectRootMenu";
    public const string InvalidInputKey = "BaseModuleInvalidInput";
    public const string UnknownErrorKey = "UnknownError";
    public const string RootStateName = "root";

    private readonly IServiceProvider _services;
    private readonly IStateRegistry _states;
    private readonly ILogger _logger;
    private readonly string _moduleName;
    private readonly ILocalizationService _localizationService;

    protected ModuleBase(ButtonLabel label, IEnumerable<Role> allowedRoles, IServiceProvider services)
    {
        var type = GetType();
        _moduleName = GetType().Name;
        _services = services;
        _states = services.GetRequiredService<IStateRegistry>();
        _localizationService = services.GetRequiredService<ILocalizationService>();
        _logger = (ILogger)services.GetRequiredService(typeof(ILogger<>).MakeGenericType(type));
        Label = label;
        AllowedRoles = FrozenSet.ToFrozenSet(allowedRoles);
    }

    protected IServiceProvider Services => _services;
    protected ILogger Logger => _logger;
    protected ILocalizationService LocalizationService => _localizationService;

    public bool IsEnabled { get; set; } = true;

    public string Name => _moduleName;

    public string RootStateId => $"{Name}:root";

    public ButtonLabel Label { get; }

    public IReadOnlySet<Role> AllowedRoles { get; }

    [MenuState(SelectRootMenuKey, StateName = RootStateName)]
    public abstract Task<StateResult> OnModuleRootAsync(ModuleStateContext ctx);

    internal async Task<ModelBindingBuilder.BindingResult> OnBindModelAsync(ModelBindContext ctx)
    {
        if (!ctx.TryGetData(out ModelBuilderData data))
        {
            var model = Activator.CreateInstance(ctx.Binding.RequestedModelType);
            if (model == null)
            {
                await ctx.ReplyAsync($"Model type '{ctx.Binding.RequestedModelType.FullName}' cannot be initialized using parameterless constructor.");
                return new(false, []);
            }
            data = new(
                ctx.Binding.RequestedModelType.FullName!,
                ctx.Binding.ModelProperties[0].Name,
                ModelBindingBuilder.ToJsonElement(model),
                false);
        }
        var builder = ModelBindingBuilder.FromData(ctx.Binding, data, Services);
        var result = await OnUpdateModelPropertyAsync(ctx, builder);

        foreach (var error in result.ValidationErrors)
        {
            if (error.ErrorMessage == null)
                continue;
            await ctx.ReplyAsync(error.ErrorMessage);
        }

        return result;
    }

    private async Task<ModelBindingBuilder.BindingResult> OnUpdateModelPropertyAsync(ModelBindContext ctx, ModelBindingBuilder bindingBuilder)
    {
        if (ctx.Message is not TextMessageContent msg)
            return new(false, [new(LocalizationService.GetString(InvalidInputKey))]);

        var (ok, val) = bindingBuilder.InputProperty.Property.PropertyType == typeof(string) ?
            (true, msg.Text) :
            ParsableInvoker.TryParseAsObject(bindingBuilder.InputProperty.Property.PropertyType, msg.Text);
        if (!ok)
            return new(false, [new(LocalizationService.GetString(InvalidInputKey))]);

        var result = bindingBuilder.AppendValue(val);
        foreach (var error in result.ValidationErrors)
        {
            if (error.ErrorMessage == null)
                continue;
            error.ErrorMessage = LocalizationService.GetString(error.ErrorMessage ?? string.Empty);
        }

        var customValidationResult = await ValidateModelAsync(ctx, bindingBuilder, result.ValidationErrors);
        return result with { HasError = !customValidationResult };
    }

    protected virtual Task<bool> ValidateModelAsync(ModelBindContext ctx, ModelBindingBuilder bindingBuilder, ICollection<ValidationResult> errors)
        => Task.FromResult(true);

    public virtual ModuleStateContext PrepareContext(RoleStateContext ctx, CancellationToken cancellationToken) =>
        new(ctx.User, ctx.User.TelegramId, ctx.Role, ctx.Message, ctx.StateData, ctx.BotClient, cancellationToken);

    protected static StateResult Retry(ModuleStateContext ctx, string? stateData, string? message)
        => new(ctx.User.State, stateData ?? ctx.StateData, message);

    protected static StateResult RetryWithMessage(ModuleStateContext ctx, string? message)
        => Retry(ctx, ctx.StateData, message);

    protected internal static StateResult RetryWith<T>(ModuleStateContext ctx, T data, string? message = null)
        => Retry(ctx, JsonSerializer.Serialize(data), message);

    protected static StateResult Retry(ModuleStateContext ctx)
        => Retry(ctx, ctx.StateData, null);

    protected static StateResult FromStart(string? stateData, string? message)
        => new("start", stateData ?? string.Empty, message);

    protected static StateResult FromStartWithMessage(string? message)
        => FromStart(string.Empty, message);

    protected static StateResult FromStartWith<T>(T data, string? message = null)
        => FromStart(JsonSerializer.Serialize(data), message);

    protected static StateResult FromStart()
        => FromStart(string.Empty, null);

    protected StateResult Fail(string? stateData, string? message)
        => FromStart(stateData ?? _localizationService.GetString(UnknownErrorKey), message);

    protected internal StateResult FailWithMessage(string? message)
        => FromStart(_localizationService.GetString(UnknownErrorKey), message);

    protected StateResult FailWith<T>(T data, string? message = null)
        => Fail(JsonSerializer.Serialize(data), message);

    protected StateResult Fail()
        => Fail(_localizationService.GetString(UnknownErrorKey), _localizationService.GetString(UnknownErrorKey));

    protected internal virtual StateResult InvalidInput(ModuleStateContext ctx)
        => Retry(ctx, ctx.StateData, _localizationService.GetString(InvalidInputKey));

    protected StateResult ToRoot(string? stateData, string? message)
        => new(RootStateId, stateData ?? string.Empty, message);

    protected StateResult ToRootWithMessage(string? message)
        => ToRoot(string.Empty, message);

    protected StateResult ToRootWith<T>(T data, string? message)
        => ToRoot(JsonSerializer.Serialize(data), message);

    protected StateResult ToRoot()
        => ToRoot(string.Empty, null);

    protected StateResult Completed(string message)
        => ToRootWithMessage(message);

    protected StateResult ToGlobalState<TModule>(string stateKey, string? stateData, string? message)
    {
        string stateId = $"{typeof(TModule).Name}:{stateKey}";
        if (!_states.TryGet(stateId, out _))
            throw new InvalidOperationException("Cannot move to the state that doesn't exist");
        return new(stateId, stateData ?? string.Empty, message);
    }

    protected StateResult ToGlobalStateWithMessage<TModule>(string stateKey, string? message)
        => ToGlobalState<TModule>(stateKey, string.Empty, message);

    protected StateResult ToGlobalStateWith<TModule, TData>(string stateKey, TData data, string? message = null)
        => ToGlobalState<TModule>(stateKey, JsonSerializer.Serialize(data), message);

    protected StateResult ToGlobalState<TModule>(string stateKey)
        => ToGlobalState<TModule>(stateKey, string.Empty, null);

    protected internal StateResult Back(ModuleStateContext ctx, string? stateData, string? message)
    {
        if (ctx.User.State == RootStateId)
            return FromStart(stateData, message);
        if (_states.TryGet(ctx.User.State, out var current))
            return new(current.ParentStateId, stateData ?? string.Empty, message);
        return ToRoot(stateData, message);
    }

    protected internal StateResult Back(ModuleStateContext ctx, string? message)
        => Back(ctx, string.Empty, message);

    protected internal StateResult BackWith<T>(ModuleStateContext ctx, T data, string? message = null)
        => Back(ctx, JsonSerializer.Serialize(data), message);

    protected internal StateResult Back(ModuleStateContext ctx)
        => Back(ctx, string.Empty, null);

    protected StateResult ToState(string stateKey, string? stateData, string? message)
    {
        string stateId = $"{Name}:{stateKey}";
        if (!_states.TryGet(stateId, out _))
            throw new InvalidOperationException("Cannot move to the state that doesn't exist");
        return new(stateId, stateData ?? string.Empty, message);
    }

    protected StateResult ToState(string stateKey)
        => ToState(stateKey, string.Empty, null);

    protected StateResult ToStateWithMessage(string stateKey, string? message)
        => ToState(stateKey, string.Empty, message);

    protected StateResult ToStateWith<T>(string stateKey, T data, string? message = null)
        => ToState(stateKey, JsonSerializer.Serialize(data), message);
}
