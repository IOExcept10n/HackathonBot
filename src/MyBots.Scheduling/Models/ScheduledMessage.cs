using System.ComponentModel.DataAnnotations;

namespace MyBots.Core.Models;

public class ScheduledMessage
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    public virtual User? User { get; set; }
    
    [Required]
    public string Message { get; set; } = string.Empty;
    
    public DateTimeOffset ScheduledTime { get; set; }
    
    public bool IsSent { get; set; }
    
    public bool IsBroadcast { get; set; }
    
    [MaxLength(128)]
    public string? JobId { get; set; }
}