using MyBots.Core.Fsm.States;
using MyBots.Core.Persistence.DTO;
using Telegram.Bot;

namespace MyBots.Modules.Common.Roles;

#pragma warning disable CS0618 // obsolete statedata attr
public record RoleStateContext(User User, Role Role, MessageContent Message, string StateData, ITelegramBotClient BotClient) : StateContext(User, Message, StateData, BotClient)
{
    public static RoleStateContext FromContext(StateContext ctx, Role role) =>
        new(ctx.User, role, ctx.Message, ctx.StateData, ctx.BotClient);
}