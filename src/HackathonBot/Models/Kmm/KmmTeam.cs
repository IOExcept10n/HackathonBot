namespace HackathonBot.Models.Kmm;

public enum MafiaRole
{
    Citizen,
    Detective,
    Doctor,
    Mafia,
    Godfather,
}

public class KmmTeam
{
    public long Id { get; set; }

    public Guid HackathonTeamId { get; set; }

    public MafiaRole Role { get; set; }

    public int Score { get; set; }

    public bool IsAlive { get; set; }

    public string Color { get; set; } = string.Empty;

    public Team HackathonTeam { get; set; } = null!;

    public ICollection<AbilityUse> AbilitiesLog { get; set; } = [];

    public ICollection<EventEntry> EventEntries { get; set; } = [];
}
