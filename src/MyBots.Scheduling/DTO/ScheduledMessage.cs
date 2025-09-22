namespace MyBots.Core.DTO;

public class ScheduledMessage
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public User? User { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset ScheduledTime { get; set; }
    public bool IsSent { get; set; }
    public bool IsBroadcast { get; set; }
    public string? JobId { get; set; }
}