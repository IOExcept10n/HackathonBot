using Telegram.Bot.Types.ReplyMarkups;

namespace MyBots.Core.Fsm;

/// <summary>
/// Represents an abstract base for actions that can be performed by the bot.
/// </summary>
public abstract record BotAction;

/// <summary>
/// Represents an action to send a text message with an optional reply markup.
/// </summary>
/// <param name="UserId">The ID of the user to send the message to.</param>
/// <param name="Text">The text content of the message.</param>
/// <param name="Markup">Optional reply markup to display with the message.</param>
public abstract record SendMessageAction(string Text, ReplyMarkup? Markup = null) : BotAction;

/// <summary>
/// Represents an action to send a text message with an optional reply keyboard.
/// </summary>
/// <param name="UserId">The ID of the user to send the message to.</param>
/// <param name="Text">The text content of the message.</param>
/// <param name="ReplyKeyboard">Optional reply keyboard markup to display with the message.</param>
public record SendTextAction(string Text, ReplyKeyboardMarkup? ReplyKeyboard = null) : SendMessageAction(Text, ReplyKeyboard);

/// <summary>
/// Represents an action to send a message with an inline keyboard.
/// </summary>
/// <param name="UserId">The ID of the user to send the message to.</param>
/// <param name="Text">The text content of the message.</param>
/// <param name="Inline">The inline keyboard markup to display with the message.</param>
public record SendInlineKeyboardAction(string Text, InlineKeyboardMarkup Inline) : SendMessageAction(Text, Inline);

/// <summary>
/// Represents an action to request user input based on a prompt configuration.
/// </summary>
/// <param name="UserId">The ID of the user to request input from.</param>
/// <param name="Request">The prompt configuration specifying the type of input requested.</param>
public record RequestPromptAction(long UserId, PromptRequest Request) : BotAction;