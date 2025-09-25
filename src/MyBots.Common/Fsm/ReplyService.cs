using MyBots.Core.Fsm.States;
using Telegram.Bot;

namespace MyBots.Core.Fsm;

public class ReplyService : IReplyService
{
    public async Task SendReplyAsync(ITelegramBotClient client, long userId, StateDefinition targetState, string? overrideReplyMessage = null, CancellationToken cancellationToken = default)
        => await targetState.Layout.SendLayoutMessageAsync(client, userId, overrideReplyMessage, cancellationToken);
}
