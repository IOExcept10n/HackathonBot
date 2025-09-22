using Telegram.Bot.Types;

namespace MyBots.Core.Fsm;

public interface IFsmDispatcher
{
    Task HandleUpdateAsync(Update update, CancellationToken ct = default);
}