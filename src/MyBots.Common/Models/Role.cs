using System.ComponentModel.DataAnnotations;

namespace MyBots.Core.Models;

public class Role
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;
    
    public virtual ICollection<User> Users { get; set; } = [];
    public virtual ICollection<ModuleRule> ModuleRules { get; set; } = [];
}