using MyBots.Core.Fsm.States;
using MyBots.Modules.Common.Roles;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = MyBots.Core.Persistence.DTO.User;

namespace MyBots.Modules.Common;

public record ModuleStateContext(User User,
                                 ChatId Chat,
                                 Role Role,
                                 MessageContent Message,
                                 string StateData,
                                 ITelegramBotClient BotClient,
                                 CancellationToken CancellationToken) : RoleStateContext(User, Role, Message, StateData, BotClient);
