using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyBots.Core.Fsm.Persistency;
using MyBots.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyBots.Core.Fsm;

public class FsmDispatcher : IFsmDispatcher
{
    private readonly IStateStore _stateStore;
    private readonly IStateRegistry _registry;
    private readonly IRoleProvider _roleProvider;
    private readonly IPromptService _promptService;
    private readonly ITelegramBotClient _bot;
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;
    private readonly IStatsCollector _stats;

    public async Task HandleUpdateAsync(Update update, CancellationToken ct = default)
    {
        var userId = GetUserId(update);
        if (userId == null) return;
        var role = await _roleProvider.GetRoleAsync(userId.Value, ct);

        // /start handling
        if (IsStartCommand(update)) { await ResetToRoot(userId.Value, role, ct); return; }

        // check prompt first
        var promptResp = await _promptService.TryResolvePromptAsync(userId.Value, update);
        // load session
        var session = await _stateStore.GetAsync(userId.Value, ct) ?? await CreateRootSession(userId.Value, role, ct);

        // if prompt resolved -> create synthetic Update (PromptResponse) or leave original update
        var effectiveUpdate = promptResp != null ? promptResp.ToUpdate() : update;

        // get definition
        if (!_registry.TryGet(session.StateId, out var def) || !_registry.IsModuleEnabled(def.Module))
        {
            // fallback to root for role
            var root = _registry.ListForRole(role).FirstOrDefault(d => d.IsRootForRole);
            session.StateId = root?.StateId ?? "default:menu";
            await _stateStore.SaveAsync(session, ct);
            def = root != null ? (StateDefinition?)root : null;
        }

        // check role allowed
        if (def != null && def.AllowedRoles.Length > 0 && !def.AllowedRoles.Contains(role))
        {
            await _bot.SendMessage(userId.Value, "Доступ запрещён", cancellationToken: ct);
            return;
        }

        // resolve handler
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
        _stats.ObserveStateDuration(session.UserId, session.StateId, sw.Elapsed);

        // apply result: save state, send actions, register prompts
        if (result.NextStateId != null)
        {
            if (result.KeepHistory) session.History.Add(new StateHistoryEntry { StateId = session.StateId, EnteredAt = DateTimeOffset.UtcNow });
            session.StateId = result.NextStateId;
            session.StateDataJson = result.NextStateDataJson;
            session.UpdatedAt = DateTimeOffset.UtcNow;
            session.Version++;
            await _stateStore.SaveAsync(session, ct);
        }

        foreach (var action in result.Actions)
        {
            await ExecuteActionAsync(action, ct);
        }
    }

    private async Task ExecuteActionAsync(BotAction action, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    private async Task<SessionState> CreateRootSession(long value, Role role, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    private bool IsStartCommand(Update update)
    {
        throw new NotImplementedException();
    }

    private async Task ResetToRoot(object value, object role, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    private long? GetUserId(Update update)
    {
        var source = update.Message?.From ?? update.CallbackQuery?.From ?? update.InlineQuery?.From;
        return source?.Id;
    }
}
