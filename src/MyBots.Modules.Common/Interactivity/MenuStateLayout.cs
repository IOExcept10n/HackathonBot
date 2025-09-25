using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyBots.Modules.Common.Interactivity;

public class MenuStateLayout : StateLayout
{
    public IEnumerable<IEnumerable<ButtonLabel>> Buttons { get; init; } = [];

    public bool ResizeKeyboard { get; init; }

    public bool OneTimeKeyboard { get; init; }

    public string? InputFieldPlaceholder { get; init; }

    public bool DisableKeyboard { get; init; }

    public override async Task SendLayoutMessageAsync(ITelegramBotClient client, ChatId chatId, string? overrideReplyMessage = null, CancellationToken cancellationToken = default)
    {
        ReplyMarkup keyboardMarkup;

        if (DisableKeyboard)
            keyboardMarkup = RemoveMarkup;
        else
            keyboardMarkup = new ReplyKeyboardMarkup()
            {
                ResizeKeyboard = ResizeKeyboard,
                OneTimeKeyboard = OneTimeKeyboard,
                InputFieldPlaceholder = InputFieldPlaceholder,
                Keyboard = from row in Buttons select (from button in row select new KeyboardButton(button.ToString()))
            };

        await client.SendMessage(
            chatId,
            overrideReplyMessage ?? MessageText,
            Telegram.Bot.Types.Enums.ParseMode.Markdown,
            replyMarkup: keyboardMarkup,
            cancellationToken: cancellationToken);
    }
}