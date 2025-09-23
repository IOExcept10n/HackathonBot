using System.ComponentModel.DataAnnotations;

namespace MyBots.Core.Models;

/// <summary>
/// Represents a user in the bot system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier of the user in the system.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Gets or sets the Telegram user identifier.
    /// </summary>
    [Required]
    public long TelegramId { get; set; }
    
    /// <summary>
    /// Gets or sets the display name of the user.
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user's role in the system.
    /// </summary>
    public Role? Role { get; set; }
    
    /// <summary>
    /// Gets or sets the current FSM state of the user.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string State { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the serialized data associated with the current FSM state.
    /// </summary>
    public string? StateData { get; set; }
    
    /// <summary>
    /// Gets or sets the user's preferred language code (e.g., "en-US").
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string Language { get; set; } = "en-US";
}