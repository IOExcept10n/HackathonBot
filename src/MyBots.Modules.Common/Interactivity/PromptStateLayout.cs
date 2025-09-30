using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyBots.Modules.Common.Interactivity;

public class PromptStateLayout(ButtonLabel cancelButton) : StateLayout
{
    private readonly ReplyKeyboardMarkup _cancelMarkup = new([[cancelButton.ToString()]]) { ResizeKeyboard = true };

    public bool AllowCancel { get; set; }

    public override async Task SendLayoutMessageAsync(ITelegramBotClient client,
                                                      ChatId chatId,
                                                      string? overrideReplyMessage = null,
                                                      CancellationToken cancellationToken = default)
        => await client.SendMessage(chatId,
                                    overrideReplyMessage ?? MessageText,
                                    replyMarkup: AllowCancel ? _cancelMarkup : RemoveMarkup,
                                    cancellationToken: cancellationToken);
}