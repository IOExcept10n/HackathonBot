using System.ComponentModel.DataAnnotations;

namespace MyBots.Core.Models;

/// <summary>
/// Represents a role that can be assigned to users in the bot system.
/// </summary>
public class Role
{
    /// <summary>
    /// Gets or sets the unique identifier of the role.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the collection of users assigned to this role.
    /// </summary>
    public virtual ICollection<User> Users { get; set; } = [];
}