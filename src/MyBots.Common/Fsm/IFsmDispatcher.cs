using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyBots.Core.Fsm;

public interface IFsmDispatcher
{
    void Configure(ITelegramBotClient client);

    Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);
}
