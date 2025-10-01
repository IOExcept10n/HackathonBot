namespace HackathonBot.Models.Kmm;

public class Quest
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long EventId { get; set; }
    public bool IsSabotage { get; set; }
    public Event Event { get; set; } = null!;
}
