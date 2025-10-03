using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using MyBots.Core.Fsm.States;
using Telegram.Bot;

namespace MyBots.Modules.Common;

#pragma warning disable CS0618 // obsolete statedata attr
public static class ModuleContextExtensions
{
    public static async Task ReplyAsync(this ModuleStateContext ctx, string message) => await ctx.BotClient.SendMessage(ctx.Chat, message);

    public static bool TryGetData<T>(this StateContext ctx, [NotNullWhen(true)][MaybeNullWhen(false)] out T data)
    {
        try
        {
            data = JsonSerializer.Deserialize<T>(ctx.StateData);
            ArgumentNullException.ThrowIfNull(data);
            return true;
        }
        catch
        {
            data = default!;
            return false;
        }
    }
}