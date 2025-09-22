using System.ComponentModel.DataAnnotations;

namespace MyBots.Core.Models;

public class User
{
    public long Id { get; set; }
    
    [Required]
    public long TelegramId { get; set; }
    
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    
    public virtual ICollection<Role> Roles { get; set; } = [];
    
    [Required]
    [MaxLength(128)]
    public string State { get; set; } = string.Empty;
    
    public string? StateData { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string Language { get; set; } = "en-US";
}