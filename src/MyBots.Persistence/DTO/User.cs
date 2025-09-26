namespace MyBots.Core.Persistence.DTO;

/// <summary>
/// Data transfer object representing a user in the persistence layer.
/// </summary>
public class User
{
    private string name = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the user in the database.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the Telegram user identifier.
    /// </summary>
    public long TelegramId { get; set; }

    /// <summary>
    /// Gets or sets the Telegram nickname of the user.
    /// </summary>
    public string Name { get => name; set => name = value.Replace("@", "").ToLowerInvariant(); }
    /// <summary>
    /// Gets or sets the current FSM state of the user.
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serialized data associated with the current FSM state.
    /// </summary>
    public string StateData { get; set; } = string.Empty;
}