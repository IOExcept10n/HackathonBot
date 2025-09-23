using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyBots.Core.Fsm.Persistency;
using MyBots.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyBots.Core.Fsm;

public class FsmDispatcher(
    IStateStore stateStore,
    IStateRegistry registry,
    IRoleProvider roleProvider,
    IPromptService promptService,
    ITelegramBotClient bot,
    IServiceProvider services,
    ILogger<FsmDispatcher> logger,
    IStatsCollector? stats = null) : IFsmDispatcher
{
    private readonly IStateStore _stateStore = stateStore;
    private readonly IStateRegistry _registry = registry;
    private readonly IRoleProvider _roleProvider = roleProvider;
    private readonly IPromptService _promptService = promptService;
    private readonly ITelegramBotClient _bot = bot;
    private readonly IServiceProvider _services = services;
    private readonly ILogger<FsmDispatcher> _logger = logger;
    private readonly IStatsCollector? _stats = stats;

    public async Task HandleUpdateAsync(Update update, CancellationToken ct = default)
    {
        try
        {
            var userId = GetUserId(update);
            if (userId == null)
            {
                _logger.LogWarning("Received update without user ID");
                return;
            }

            var role = await _roleProvider.GetRoleAsync(userId.Value, ct);
            if (role == null)
            {
                _logger.LogWarning("User {UserId} has no role assigned", userId.Value);
                return;
            }

            // Handle /start command -> reset to role root
            if (IsStartCommand(update))
            {
                await ResetToRoot(userId.Value, role, ct);
                return;
            }

            // Check prompt first
            var promptResp = await _promptService.TryResolvePromptAsync(userId.Value, update);

            // Load or create session
            var session = await _stateStore.GetAsync(userId.Value, ct) ?? await CreateRootSession(userId.Value, role, ct);

            // If prompt resolved -> use synthetic update produced by prompt
            var effectiveUpdate = promptResp.ToUpdate() ?? update;

            // Resolve state definition, falling back to role root if not found or module disabled
            if (!_registry.TryGet(session.StateId, out var def) || !_registry.IsModuleEnabled(def.Module))
            {
                var root = _registry.ListForRole(role).FirstOrDefault(d => d.IsRootForRole);
                if (root == null)
                {
                    _logger.LogError("No root state found for role {RoleName}", role.Name);
                    await _bot.SendMessage(userId.Value, "Произошла ошибка (нет корневого состояния).", cancellationToken: ct);
                    return;
                }

                session.StateId = root.StateId;
                session.StateDataJson = null;
                session.UpdatedAt = DateTimeOffset.UtcNow;
                session.Version++;
                await _stateStore.SaveAsync(session, ct);
                def = root;
            }

            // Access check
            if (def == null || (def.AllowedRoles.Length > 0 && !def.AllowedRoles.Contains(role)))
            {
                _logger.LogWarning("User {UserId} with role {RoleName} denied access to state {StateId}", userId.Value, role.Name, def?.StateId);
                await _bot.SendMessage(userId.Value, "Доступ запрещён", cancellationToken: ct);
                return;
            }

            // Resolve handler instance
            var handler = (IStateHandler)_services.GetRequiredService(def.HandlerType);

            var ctx = new StateContext
            {
                Update = effectiveUpdate,
                Session = session,
                Role = role,
                Services = _services,
                CancellationToken = ct,
                Prompt = _promptService,
                Keyboard = _services.GetRequiredService<IKeyboardBuilder>()
            };

            var sw = Stopwatch.StartNew();
            var result = await handler.HandleAsync(ctx);
            sw.Stop();
            _stats?.ObserveStateDuration(session.UserId, session.StateId, sw.Elapsed);

            // Apply transition
            if (result.NextStateId != null)
            {
                // validate next state exists and accessible
                if (!_registry.TryGet(result.NextStateId, out var nextDef))
                {
                    _logger.LogError("Invalid next state {StateId}", result.NextStateId);
                    // optionally notify user
                }
                else if (nextDef.AllowedRoles.Length > 0 && !nextDef.AllowedRoles.Contains(role))
                {
                    _logger.LogWarning("User {UserId} with role {RoleName} tried to transition to forbidden state {StateId}",
                        userId.Value, role.Name, nextDef.StateId);
                    await _bot.SendMessage(userId.Value, "Доступ запрещён", cancellationToken: ct);
                    return;
                }
                else
                {
                    if (result.KeepHistory)
                    {
                        session.History.Add(new StateHistoryEntry { StateId = session.StateId, EnteredAt = DateTimeOffset.UtcNow });
                    }

                    session.StateId = result.NextStateId;
                    session.StateDataJson = result.NextStateDataJson;
                    session.UpdatedAt = DateTimeOffset.UtcNow;
                    session.Version++;
                    await _stateStore.SaveAsync(session, ct);
                }
            }

            // Execute actions produced by handler
            foreach (var action in result.Actions)
            {
                try
                {
                    await ExecuteActionAsync(session.UserId, action, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing bot action {ActionType}", action.GetType());
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling update");
        }
    }

    private async Task ExecuteActionAsync(ChatId chatId, BotAction action, CancellationToken ct)
    {
        // Basic implementation — expand based on BotAction shape
        switch (action)
        {
            case SendMessageAction sendText:
                await _bot.SendMessage(
                    chatId: chatId,
                    text: sendText.Text ?? string.Empty,
                    replyMarkup: sendText.Markup,
                    cancellationToken: ct);
                break;

            //case BotActionType.EditMessage:
            //    await _bot.EditMessageTextAsync(
            //        chatId: action.ChatId ?? throw new InvalidOperationException("ChatId required"),
            //        messageId: action.MessageId ?? throw new InvalidOperationException("MessageId required"),
            //        text: action.Text ?? string.Empty,
            //        replyMarkup: action.ReplyMarkup,
            //        cancellationToken: ct);
            //    break;

            //case BotActionType.DeleteMessage:
            //    if (action.ChatId != null && action.MessageId != null)
            //        await _bot.DeleteMessageAsync(action.ChatId.Value, action.MessageId.Value, ct);
            //    break;

            // Add other action types as needed
            default:
                _logger.LogWarning("Unknown action type: {ActionType}", action.GetType());
                break;
        }
    }

    private async Task<SessionState> CreateRootSession(long userId, Role role, CancellationToken ct)
    {
        var root = _registry.ListForRole(role).FirstOrDefault(d => d.IsRootForRole);
        var stateId = root?.StateId ?? "default:menu";
        var session = new SessionState
        {
            UserId = userId,
            StateId = stateId,
            StateDataJson = null,
            History = [],
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        };

        await _stateStore.SaveAsync(session, ct);
        return session;
    }

    private static bool IsStartCommand(Update update)
    {
        // Recognize /start in message text or deep-link start via entities
        var msg = update.Message;
        if (msg == null || string.IsNullOrWhiteSpace(msg.Text)) return false;
        var text = msg.Text.Trim();
        if (text.StartsWith("/start", StringComparison.OrdinalIgnoreCase)) return true;
        return false;
    }

    private async Task ResetToRoot(long userId, Role role, CancellationToken ct)
    {
        var root = _registry.ListForRole(role).FirstOrDefault(d => d.IsRootForRole);
        if (root == null)
        {
            _logger.LogError("No root state for role {RoleName} when resetting /start", role.Name);
            await _bot.SendMessage(userId, "Произошла ошибка.", cancellationToken: ct);
            return;
        }

        var session = new SessionState
        {
            UserId = userId,
            StateId = root.StateId,
            StateDataJson = null,
            History = new List<StateHistoryEntry>(),
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        };

        await _stateStore.SaveAsync(session, ct);

        // Optionally send root welcome message — use prompt service or send static text
        await _bot.SendMessage(userId, "Главное меню", cancellationToken: ct);
    }

    private static long? GetUserId(Update update)
    {
        return update.Message?.From?.Id
            ?? update.CallbackQuery?.From?.Id
            ?? update.InlineQuery?.From?.Id
            ?? update.ChosenInlineResult?.From?.Id
            ?? update.PreCheckoutQuery?.From?.Id
            ?? update.ShippingQuery?.From?.Id
            ?? update.MyChatMember?.From?.Id
            ?? update.ChatMember?.From?.Id
            ?? update.ChatJoinRequest?.From?.Id;
    }
}
