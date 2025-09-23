using Telegram.Bot.Types.ReplyMarkups;

namespace MyBots.Core.Fsm;

/// <summary>
/// Provides methods for building and parsing Telegram keyboard layouts.
/// </summary>
public interface IKeyboardBuilder
{
    /// <summary>
    /// Builds a reply keyboard markup from a matrix of text buttons.
    /// Each inner enumerable represents a row of buttons.
    /// </summary>
    /// <param name="rows">The matrix of button texts organized in rows.</param>
    /// <param name="resizeKeyboard">Whether to resize the keyboard to fit the screen.</param>
    /// <param name="oneTimeKeyboard">Whether to hide the keyboard after first use.</param>
    /// <returns>A configured reply keyboard markup.</returns>
    ReplyKeyboardMarkup BuildReplyKeyboard(IEnumerable<IEnumerable<string>> rows,
        bool resizeKeyboard = true, bool oneTimeKeyboard = false);

    /// <summary>
    /// Builds an inline keyboard markup from a matrix of (text, callbackData) pairs.
    /// Each inner enumerable represents a row of buttons.
    /// </summary>
    /// <param name="rows">The matrix of button configurations organized in rows.</param>
    /// <returns>A configured inline keyboard markup.</returns>
    InlineKeyboardMarkup BuildInlineKeyboard(IEnumerable<IEnumerable<(string Text, string CallbackData)>> rows);

    /// <summary>
    /// Parses callback data according to the accepted format (optional).
    /// Returns null if parsing fails.
    /// </summary>
    /// <param name="callbackData">The callback data to parse.</param>
    /// <returns>A tuple containing the state, action, and ID if parsing succeeds; otherwise null.</returns>
    (string? State, string? Action, string? Id)? ParseCallbackPayload(string callbackData);
}
