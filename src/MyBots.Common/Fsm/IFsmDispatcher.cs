using Telegram.Bot.Types;

namespace MyBots.Core.Fsm;

/// <summary>
/// Defines the main entry point for processing Telegram updates through the FSM system.
/// </summary>
public interface IFsmDispatcher
{
    /// <summary>
    /// Processes an incoming Telegram update through the FSM, executing appropriate state handlers and actions.
    /// </summary>
    /// <param name="update">The Telegram update to process.</param>
    /// <param name="ct">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleUpdateAsync(Update update, CancellationToken ct = default);
}