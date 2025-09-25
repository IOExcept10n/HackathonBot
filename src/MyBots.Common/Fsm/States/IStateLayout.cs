using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyBots.Core.Fsm.States;

public interface IStateLayout
{
    Task SendLayoutMessageAsync(ITelegramBotClient client, ChatId chatId, string? overrideReplyMessage = null, CancellationToken cancellationToken = default);
}
