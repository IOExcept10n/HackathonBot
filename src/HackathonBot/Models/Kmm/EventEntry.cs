namespace HackathonBot.Models.Kmm;

public class EventEntry
{
    public long Id { get; set; }
    public long KmmTeamId { get; set; }
    public long QuestId { get; set; }
    public bool? IsQuestCompleted { get; set; }
    public int Place { get; set; }
    public KmmTeam Team { get; set; } = null!;
    public Quest Quest { get; set; } = null!;
}
