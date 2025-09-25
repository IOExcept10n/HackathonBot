using MyBots.Core.Fsm.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyBots.Modules.Common.Interactivity;

public abstract class StateLayout : IStateLayout
{
    protected static readonly ReplyMarkup RemoveMarkup = new ReplyKeyboardRemove();

    public string MessageText { get; init; } = string.Empty;

    public abstract Task SendLayoutMessageAsync(ITelegramBotClient client, ChatId chatId, string? overrideReplyMessage = null, CancellationToken cancellationToken = default);
}