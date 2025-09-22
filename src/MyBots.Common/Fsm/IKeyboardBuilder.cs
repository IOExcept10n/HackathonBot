using Telegram.Bot.Types.ReplyMarkups;

namespace MyBots.Core.Fsm;

public interface IKeyboardBuilder
{
    /// <summary>
    /// Построить ReplyKeyboardMarkup из матрицы текстовых кнопок.
    /// Каждая внутренняя IEnumerable — одна строка кнопок.
    /// </summary>
    ReplyKeyboardMarkup BuildReplyKeyboard(IEnumerable<IEnumerable<string>> rows,
        bool resizeKeyboard = true, bool oneTimeKeyboard = false);

    /// <summary>
    /// Построить InlineKeyboardMarkup из матрицы пар (text, callbackData).
    /// Каждая внутренняя IEnumerable — одна строка кнопок.
    /// </summary>
    InlineKeyboardMarkup BuildInlineKeyboard(IEnumerable<IEnumerable<(string Text, string CallbackData)>> rows);

    /// <summary>
    /// Разобрать callbackData по принятому формату (опционально).
    /// Возвращает null при неудаче.
    /// </summary>
    (string? State, string? Action, string? Id)? ParseCallbackPayload(string callbackData);
}
