using System.ComponentModel.DataAnnotations;
using MyBots.Core.Models;

namespace MyBots.Core.Models;

/// <summary>
/// Represents a message scheduled to be sent at a specific time.
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
    /// Gets or sets the navigation property for the recipient user.
    /// </summary>
    public virtual User? User { get; set; }
    
    /// <summary>
    /// Gets or sets the text content of the message.
    /// </summary>
    [Required]
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
    [MaxLength(128)]
    public string? JobId { get; set; }
}