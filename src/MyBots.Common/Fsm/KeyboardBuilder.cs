using Telegram.Bot.Types.ReplyMarkups;

namespace MyBots.Core.Fsm;

/// <summary>
/// Default implementation of the keyboard builder.
/// </summary>
public class KeyboardBuilder : IKeyboardBuilder
{
    public ReplyKeyboardMarkup BuildReplyKeyboard(IEnumerable<IEnumerable<string>> rows,
        bool resizeKeyboard = true, bool oneTimeKeyboard = false)
    {
        var keyboardRows = rows
            .Select(row => row
                .Select(text => new KeyboardButton(text))
                .ToArray())
            .ToArray();

        return new ReplyKeyboardMarkup(keyboardRows)
        {
            ResizeKeyboard = resizeKeyboard,
            OneTimeKeyboard = oneTimeKeyboard
        };
    }

    public InlineKeyboardMarkup BuildInlineKeyboard(IEnumerable<IEnumerable<(string Text, string CallbackData)>> rows)
    {
        var keyboardRows = rows
            .Select(row => row
                .Select(button => InlineKeyboardButton.WithCallbackData(
                    button.Text, button.CallbackData))
                .ToArray())
            .ToArray();

        return new InlineKeyboardMarkup(keyboardRows);
    }

    public (string? State, string? Action, string? Id)? ParseCallbackPayload(string callbackData)
    {
        var parts = callbackData.Split(':', 3);
        if (parts.Length < 2) return null;

        return (
            State: parts[0],
            Action: parts[1],
            Id: parts.Length > 2 ? parts[2] : null
        );
    }
}