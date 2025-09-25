using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyBots.Modules.Common.Interactivity;

public class PromptStateLayout : StateLayout
{
    public override async Task SendLayoutMessageAsync(ITelegramBotClient client, ChatId chatId, string? overrideReplyMessage = null, CancellationToken cancellationToken = default)
    {
        await client.SendMessage(chatId, overrideReplyMessage ?? MessageText, Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: RemoveMarkup, cancellationToken: cancellationToken);
    }
}