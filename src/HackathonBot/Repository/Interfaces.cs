using HackathonBot.Models;
using MyBots.Core.Persistence.Repository;

namespace HackathonBot.Repository;

public interface IParticipantRepository : IRepository<Participant>
{
    Task<Participant?> FindByTelegramIdAsync(long telegramId, CancellationToken ct = default);

    Task<Participant?> FindByUsernameAsync(string username, CancellationToken ct = default);

    Task<IList<Participant>> GetByTeamIdAsync(Guid teamId, CancellationToken ct = default);

    Task<bool> IsLeaderAsync(Guid participantId, CancellationToken ct = default);
}

public interface ITeamRepository : IRepository<Team>
{
    Task<Team?> GetWithMembersAsync(Guid teamId, CancellationToken ct = default);

    Task<Team?> FindByNameAsync(string name, CancellationToken ct = default);
}

public interface ISubmissionRepository : IRepository<Submission>
{
    Task<Submission?> GetByTeamIdAsync(Guid teamId, CancellationToken ct = default);

    Task<IList<Submission>> GetByCaseAsync(Case c, CancellationToken ct = default);
}

public interface IBotUserRoleRepository : IRepository<BotUserRole>
{
    Task<BotUserRole?> FindByTelegramIdAsync(long telegramId, CancellationToken ct = default);

    Task<BotUserRole?> FindByUsernameAsync(string username, CancellationToken ct = default);
}