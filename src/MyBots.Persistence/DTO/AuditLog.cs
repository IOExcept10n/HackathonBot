namespace MyBots.Persistence.DTO;

public class AuditLog
{
    public long Id { get; set; }
    public DateTime Timestamp { get; set; }
    public long UserId { get; set; }
    public User? User { get; set; }
    public string Action { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}