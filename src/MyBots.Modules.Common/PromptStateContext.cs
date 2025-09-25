using MyBots.Core.Fsm.States;
using MyBots.Modules.Common.Roles;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = MyBots.Core.Persistence.DTO.User;

namespace MyBots.Modules.Common;

public record PromptStateContext<T>(
    User User,
    ChatId Chat,
    Role Role,
    MessageContent Message,
    Optional<T> MessageInput,
    string StateData,
    ITelegramBotClient BotClient,
    CancellationToken CancellationToken) : ModuleStateContext(User, Chat, Role, Message, StateData, BotClient, CancellationToken)
    where T : IParsable<T>;
