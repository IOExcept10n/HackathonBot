namespace MyBots.Persistence.DTO;

public class User
{
    public long Id { get; set; }
    public long TelegramId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Role> Roles { get; set; } = [];
    public string State { get; set; } = string.Empty;
    public string StateData { get; set; } = string.Empty;
}