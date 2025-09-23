using MyBots.Core.Fsm.Persistency;
using MyBots.Core.Models;
using Telegram.Bot.Types;

namespace MyBots.Core.Fsm;

/// <summary>
/// Represents the context available to state handlers during FSM processing.
/// </summary>
public class StateContext
{
    /// <summary>
    /// Gets the Telegram update that triggered the state processing.
    /// </summary>
    public Update Update { get; init; } = default!;

    /// <summary>
    /// Gets the current session state containing user data and state information.
    /// </summary>
    public SessionState Session { get; init; } = default!;

    /// <summary>
    /// Gets the user's role for access control and permission validation.
    /// </summary>
    public Role Role { get; init; } = default!;

    /// <summary>
    /// Gets the service provider for dependency injection within state handlers.
    /// </summary>
    public IServiceProvider Services { get; init; } = default!;

    /// <summary>
    /// Gets the cancellation token to support cancellation of async operations.
    /// </summary>
    public CancellationToken CancellationToken { get; init; }

    /// <summary>
    /// Gets the service for accessing localized prompt messages.
    /// </summary>
    public IPromptService Prompt { get; init; } = default!;

    /// <summary>
    /// Gets the service for building Telegram keyboard layouts.
    /// </summary>
    public IKeyboardBuilder Keyboard { get; init; } = default!;
}