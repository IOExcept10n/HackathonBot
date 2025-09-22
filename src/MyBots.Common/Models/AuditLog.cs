using System.ComponentModel.DataAnnotations;

namespace MyBots.Core.Models;

public class AuditLog
{
    public long Id { get; set; }
    
    public DateTime Timestamp { get; set; }
    
    public long UserId { get; set; }
    public virtual User? User { get; set; }
    
    [Required]
    [MaxLength(128)]
    public string Action { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(256)]
    public string ModuleName { get; set; } = string.Empty;
    
    public string Details { get; set; } = string.Empty;
}