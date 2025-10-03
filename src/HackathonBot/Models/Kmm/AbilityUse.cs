namespace HackathonBot.Models.Kmm;

public enum Ability
{
    None,

    #region Common
    Vote,
    #endregion

    #region Citizen
    WorkEthic,
    #endregion

    #region Mafia
    DataLeak,
    HackerAttack,
    #endregion

    #region Detective
    Check,
    #endregion

    #region Doctor
    Defense,
    Firewall,
    #endregion

    #region Godfather
    Slander,
    #endregion
}

public class AbilityUse
{
    public long Id { get; set; }
    public long TeamId { get; set; }
    public Ability Ability { get; set; }
    public DateTime UsedAt { get; set; }
    public KmmTeam Team { get; set; } = null!;
    public long? TargetTeamId { get; set; }
    public KmmTeam? TargetTeam { get; set; } = null!;
}
