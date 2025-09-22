using Telegram.Bot.Types.ReplyMarkups;

namespace MyBots.Core.Fsm;

public abstract record BotAction;
public record SendTextAction(long UserId, string Text, ReplyKeyboardMarkup? ReplyKeyboard = null) : BotAction;
public record SendInlineKeyboardAction(long UserId, string Text, InlineKeyboardMarkup Inline) : BotAction;
public record RequestPromptAction(long UserId, PromptRequest Request) : BotAction;