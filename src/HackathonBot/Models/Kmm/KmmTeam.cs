namespace HackathonBot.Models.Kmm;

public enum MafiaRole
{
    Citizen,
    Mafia,
    Detective,
    Doctor,
    Godfather,
}

public class KmmTeam
{
    public long Id { get; set; }

    public Guid HackathonTeamId { get; set; }

    public MafiaRole Role { get; set; }

    public int Score { get; set; }

    public bool IsAlive { get; set; }

    public Team HackathonTeam { get; set; } = null!;

    public ICollection<AbilityUse> AbilitiesLog { get; set; } = [];

    public ICollection<EventEntry> EventEntries { get; set; } = [];
}
