using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace MyBots.Core.Fsm;

/// <summary>
/// Represents a request for user input with specific validation criteria.
/// </summary>
public class PromptRequest
{
    /// <summary>
    /// Gets the timeout duration for waiting for user input. Default is infinite.
    /// </summary>
    public TimeSpan Timeout { get; init; } = System.Threading.Timeout.InfiniteTimeSpan;

    /// <summary>
    /// Gets a value indicating whether file attachments are accepted. Default is false.
    /// </summary>
    public bool AcceptFiles { get; init; } = false;

    /// <summary>
    /// Gets a value indicating whether text input is accepted. Default is true.
    /// </summary>
    public bool AcceptText { get; init; } = true;

    /// <summary>
    /// Gets an optional regular expression for filtering text input.
    /// </summary>
    public Regex? InputFilter { get; init; } = null;
}

/// <summary>
/// Represents a response to a user input prompt.
/// </summary>
public class PromptResponse
{
    /// <summary>
    /// Gets a value indicating whether the prompt was canceled.
    /// </summary>
    public bool Canceled { get; init; }

    /// <summary>
    /// Gets the Telegram update containing the user's response.
    /// </summary>
    public required Update Update { get; init; }
}

/// <summary>
/// Provides services for managing user input prompts.
/// </summary>
public interface IPromptService
{
    /// <summary>
    /// Registers a new prompt request for a user.
    /// </summary>
    /// <param name="userId">The ID of the user to prompt.</param>
    /// <param name="request">The prompt request configuration.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RegisterPromptAsync(long userId, PromptRequest request);

    /// <summary>
    /// Attempts to resolve a prompt with an incoming update.
    /// </summary>
    /// <param name="userId">The ID of the user who sent the update.</param>
    /// <param name="update">The incoming Telegram update.</param>
    /// <returns>A prompt response if the update matches a prompt; otherwise, null.</returns>
    Task<PromptResponse?> TryResolvePromptAsync(long userId, Update update);

    /// <summary>
    /// Cancels an active prompt for a user.
    /// </summary>
    /// <param name="userId">The ID of the user whose prompt should be canceled.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CancelPromptAsync(long userId);
}
