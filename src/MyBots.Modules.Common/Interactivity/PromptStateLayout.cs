using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyBots.Modules.Common.Interactivity;

public class PromptStateLayout : StateLayout
{
    public override async Task SendLayoutMessageAsync(ITelegramBotClient client, ChatId chatId, string? overrideReplyMessage = null, CancellationToken cancellationToken = default)
    {
        await client.SendMessage(chatId, overrideReplyMessage ?? MessageText, replyMarkup: RemoveMarkup, cancellationToken: cancellationToken);
    }
}