namespace HackathonBot.Models.Kmm;

public enum Ability
{
    None,

    #region Common
    Vote,
    #endregion

    #region Citizen
    WorkEthic,
    Waterfall,
    #endregion

    #region Mafia
    Sabotage,
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
    KillOrder,
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
}
