using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace MyBots.Core.Fsm;

public class PromptRequest
{
    public TimeSpan Timeout { get; init; } = System.Threading.Timeout.InfiniteTimeSpan;
    public bool AcceptFiles { get; init; } = false;
    public bool AcceptText { get; init; } = true;
    public Regex? InputFilter { get; init; } = null;
}

public class PromptResponse
{
    public bool Canceled { get; init; }
    public required Update Update { get; init; }
}

public interface IPromptService
{
    Task RegisterPromptAsync(long userId, PromptRequest request);
    Task<PromptResponse?> TryResolvePromptAsync(long userId, Update update);
    Task CancelPromptAsync(long userId);
}
