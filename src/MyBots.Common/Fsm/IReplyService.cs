using MyBots.Core.Fsm.States;
using Telegram.Bot;

namespace MyBots.Core.Fsm;

/// <summary>
/// Performs sending messages to prompt users for the FSM.
/// </summary>
public interface IReplyService
{
    /// <summary>
    /// Sends message with prompt for the next state.
    /// </summary>
    /// <param name="client">An instance of the telegram bot client to send reply with.</param>
    /// <param name="userId">An ID of the user to reply to.</param>
    /// <param name="targetState">A definition for a state that will respond to current prompt.</param>
    /// <param name="overrideReplyMessage">A message to write instead of target state layout message.</param>
    /// <param name="cancellationToken">An instance of the <see cref="CancellationToken"/> to cancel operation.</param>
    /// <returns>An asynchronous operation representing message sending process.</returns>
    Task SendReplyAsync(ITelegramBotClient client, long userId, StateDefinition targetState, string? overrideReplyMessage = null, CancellationToken cancellationToken = default);
}