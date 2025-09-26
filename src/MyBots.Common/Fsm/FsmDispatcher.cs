using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyBots.Core.Fsm.States;
using MyBots.Core.Persistence.Repository;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyBots.Core.Fsm;

public class FsmDispatcher(
    IStateRegistry registry,
    IStateHandlerProvider handlerProvider,
    IReplyService replyService,
    IUserStateService userStates,
    IUserRepository users,
    ILogger<FsmDispatcher> logger) : IFsmDispatcher
{
    private readonly IStateHandlerProvider _handlerProvider = handlerProvider;
    private readonly ILogger<FsmDispatcher> _logger = logger;
    private readonly IStateRegistry _registry = registry;
    private readonly IReplyService _replyService = replyService;
    private readonly IUserRepository _users = users;
    private readonly IUserStateService _userStates = userStates;

    private ITelegramBotClient? _client;

    [MemberNotNullWhen(true, nameof(_client))]
    public bool IsConfigured => _client != null;

    public void Configure(ITelegramBotClient client) => _client = client;

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken) => await (update.Type switch
    {
        UpdateType.Message => HandleMessageAsync(update.Message!, cancellationToken),
        //UpdateType.InlineQuery => HandleInlineQueryAsync(update.InlineQuery!, cancellationToken),
        //UpdateType.CallbackQuery => HandleCallbackQueryAsync(update.CallbackQuery!, cancellationToken),
        _ => Task.CompletedTask,
    });

    private async Task<StateDefinition> HandleCancelCommandAsync(StateContext ctx, StateDefinition currentState)
    {
        if (currentState.IsRoot)
            return currentState;
        if (currentState.IsSubRoot)
            return await _userStates.GetUserRootStateAsync(ctx.User);

        if (!_registry.TryGet(currentState.ParentStateId, out var parent))
        {
            parent = await _userStates.GetUserRootStateAsync(ctx.User);
        }

        return parent;
    }

    private async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
    {
        if (!IsConfigured)
            throw new InvalidOperationException("Cannot handle incoming messages when bot client is not configured.");

        if (message.From == null)
            return;

        var user = await _users.GetOrCreateByTelegramIdAsync(message.From.Id, message.From.Username?.ToLowerInvariant() ?? string.Empty, cancellationToken);
        var ctx = new StateContext(user, message.GetContent(), user.StateData, _client);

        if (!_registry.TryGet(user.State, out var def))
        {
            def = await _userStates.GetUserRootStateAsync(user);
        }

        _logger.LogInformation("Got user state {State}, trying to handle", user.State);

        var nextState = await TryHandleCommandAsync(message, ctx, def);
        string? nextMessage = null;

        // Branch if no commands handled
        if (nextState == null)
        {
            if (!_handlerProvider.TryGetHandler(def, out var handler))
            {
                _logger.LogError("Couldn't get handler for the defined state {State}", def.StateId);
                return;
            }

            var result = await handler.ExecuteAsync(ctx, cancellationToken);
            nextMessage = result.OverrideNextStateMessage;

            if (!_registry.TryGet(result.NextStateId, out nextState))
            {
                nextState = await _userStates.GetUserRootStateAsync(user);
            }

            user.StateData = result.NextStateData;
        }

        user.State = nextState.StateId;
        await _users.UpdateAsync(user, cancellationToken);
        await _users.SaveChangesAsync(cancellationToken);

        await _replyService.SendReplyAsync(_client, user.TelegramId, nextState, nextMessage, cancellationToken);
    }

    private async Task<StateDefinition> HandleStartCommandAsync(StateContext ctx)
    {
        ctx.User.StateData = string.Empty;
        return await _userStates.GetUserRootStateAsync(ctx.User);
    }

    private async Task<StateDefinition?> TryHandleCommandAsync(Message message, StateContext ctx, StateDefinition currentState) => message.Text switch
    {
        "/cancel" => await HandleCancelCommandAsync(ctx, currentState),
        "/start" => await HandleStartCommandAsync(ctx),
        _ => null,
    };
}