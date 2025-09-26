using Telegram.Bot;

namespace MyBots.Modules.Common
{
    public static class ModuleContextExtensions
    {
        public static async Task ReplyAsync(this ModuleStateContext ctx, string message) => await ctx.BotClient.SendMessage(ctx.Chat, message);
    }
}