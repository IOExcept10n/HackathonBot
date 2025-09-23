using MyBots.Core.Models;

namespace MyBots.Core.DTO;

/// <summary>
/// Data transfer object representing a scheduled message.
/// </summary>
public class ScheduledMessage
{
    /// <summary>
    /// Gets or sets the unique identifier for the scheduled message.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user who will receive the message.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Gets or sets the user who will receive the message.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets the text content of the message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time when the message should be sent.
    /// </summary>
    public DateTimeOffset ScheduledTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message has been sent.
    /// </summary>
    public bool IsSent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is a broadcast message sent to multiple users.
    /// </summary>
    public bool IsBroadcast { get; set; }

    /// <summary>
    /// Gets or sets the ID of the associated scheduler job.
    /// </summary>
    public string? JobId { get; set; }
}