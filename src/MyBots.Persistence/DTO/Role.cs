namespace MyBots.Persistence.DTO;

/// <summary>
/// Data transfer object representing a role in the persistence layer.
/// </summary>
public class Role
{
    /// <summary>
    /// Gets or sets the unique identifier of the role.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the unique name of the role.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of users assigned to this role.
    /// </summary>
    public ICollection<User> Users { get; set; } = [];
}