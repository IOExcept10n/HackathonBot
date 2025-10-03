using HackathonBot.Models;
using HackathonBot.Models.Kmm;
using HackathonBot.Modules;

namespace HackathonBot.Services;

public class GameStats
{
    public int AliveMafia { get; set; }
    public int AlivePeaceful { get; set; }
    public MafiaSide? Winner { get; set; }
    public int TotalAlive => AliveMafia + AlivePeaceful;

    public enum MafiaSide
    {
        None,
        Citizens,
        Mafia
    }
}

public record DayResult
{
    public KmmTeam? KilledByMafia { get; init; }
    public KmmTeam? LynchedByCitizens { get; init; }
    public DetectiveResult? DetectiveCheckResult { get; init; }

    public record DetectiveResult(KmmTeam? Target, bool? IsMafia);
}

public record VoteDto(KmmTeam VoterTeam, KmmTeam TargetTeam);

public interface IKmmGameService
{
    Task InitializeGameAsync(IEnumerable<Team> participatingTeams, KmmConfig options, CancellationToken ct = default);
    Task<DayResult> SimulateDayAsync(CancellationToken ct = default);
    Task<GameStats> GetGameStatsAsync(CancellationToken ct = default);
    Task<bool> CanUseAbilityAsync(KmmTeam team, Ability ability, CancellationToken ct = default);
    Task<KmmTeam?> GetKmmTeamByTelegramIdAsync(long telegramId, CancellationToken ct = default);
    Task<IEnumerable<VoteDto>> GetTodayVotesAsync(CancellationToken ct = default);
    Task<IEnumerable<AbilityUse>> GetTodayAbilityUsesAsync(CancellationToken ct = default);
}
