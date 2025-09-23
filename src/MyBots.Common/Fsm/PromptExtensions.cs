using Telegram.Bot.Types;

namespace MyBots.Core.Fsm
{
    public static class PromptExtensions
    {
        public static Update? ToUpdate(this PromptResponse? response)
        {
            if (response?.Canceled == false)
                return response.Update;
            return null;
        }
    }
}