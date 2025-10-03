using MyBots.Core.Persistence.DTO;
using Telegram.Bot;

namespace MyBots.Core.Fsm.States;

public record StateContext(
    User User,
    MessageContent Message,
    [property: Obsolete("Do not use this property directly, use StateContext.TryGetData() instead. This property is JSON-serialized field to store in database.")]
    string StateData,
    ITelegramBotClient BotClient);

public abstract record MessageContent 
{
    public static UnknownMessageContent Unknown { get; } = new();
}
public sealed record UnknownMessageContent : MessageContent;
public record TextMessageContent(string Text) : MessageContent;
public record FileMessageContent(string FileId) : MessageContent;
