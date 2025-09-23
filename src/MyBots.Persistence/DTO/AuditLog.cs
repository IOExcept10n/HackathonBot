namespace MyBots.Persistence.DTO;

/// <summary>
/// Data transfer object representing an audit log entry in the persistence layer.
/// </summary>
public class AuditLog
{
    /// <summary>
    /// Gets or sets the unique identifier of the audit log entry.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the action was performed.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who performed the action.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property for the user who performed the action.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets the name of the action that was performed.
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the module where the action was performed.
    /// </summary>
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional details about the performed action.
    /// </summary>
    public string Details { get; set; } = string.Empty;
}