using MyBots.Core.Fsm.Persistency;
using MyBots.Core.Models;
using Telegram.Bot.Types;

namespace MyBots.Core.Fsm;

public class StateContext
{
    public Update Update { get; init; } = default!;
    public SessionState Session { get; init; } = default!;
    public Role Role { get; init; } = default!;
    public IServiceProvider Services { get; init; } = default!;
    public CancellationToken CancellationToken { get; init; }
    public IPromptService Prompt { get; init; } = default!;
    public IKeyboardBuilder Keyboard { get; init; } = default!;
}