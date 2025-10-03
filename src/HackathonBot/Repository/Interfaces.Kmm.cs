using MyBots.Core.Persistence.Repository;
using HackathonBot.Models.Kmm;

namespace HackathonBot.Repository;

public interface IKmmTeamRepository : IRepository<KmmTeam>
{
    Task<KmmTeam?> GetByIdWithLogsAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<KmmTeam>> GetAliveTeamsAsync(CancellationToken ct = default);
}

public interface IAbilityUseRepository : IRepository<AbilityUse>
{
    Task<IEnumerable<AbilityUse>> GetByTeamIdAsync(long teamId, CancellationToken ct = default);
    Task<IEnumerable<AbilityUse>> GetByAbilityAsync(Ability ability, CancellationToken ct = default);
    Task<AbilityUse?> GetLastUseAsync(long teamId, Ability ability, CancellationToken ct = default);
}

public interface IEventRepository : IRepository<Event>
{
    Task<Event?> FindByNameAsync(string name, CancellationToken ct = default);
    Task<Event?> GetByIdWithQuestsAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<Event>> GetAllWithQuestsAsync(CancellationToken ct = default);
}

public interface IQuestRepository : IRepository<Quest>
{
    Task<Quest?> FindByNameAsync(long eventId, string name, CancellationToken ct = default);
    Task<IEnumerable<Quest>> GetByEventIdAsync(long eventId, CancellationToken ct = default);
    Task<IEnumerable<Quest>> GetSabotagesByEventIdAsync(long eventId, CancellationToken ct = default);
}

public interface IEventEntryRepository : IRepository<EventEntry>
{
    Task<IEnumerable<EventEntry>> GetByQuestIdAsync(long questId, CancellationToken ct = default);
    Task<IEnumerable<EventEntry>> GetByTeamIdAsync(long teamId, CancellationToken ct = default);
    Task<EventEntry?> GetEntryAsync(long questId, long teamId, CancellationToken ct = default);
}

public interface IBankRepository : IRepository<Bank>
{
    Task<Bank?> FindByKeyAsync(string key, CancellationToken ct = default);
    Task<int> GetCoinsAsync(string key, CancellationToken ct = default);
    Task<int> GetParticipantCoinsAsync(CancellationToken ct = default);
    Task SetCoinsAsync(string key, int value, CancellationToken ct = default);
    Task SetParticipantCoinsAsync(int value, CancellationToken ct = default);
}

public interface IEventAuditRepository : IRepository<EventAuditEntry>
{
    Task<IEnumerable<EventAuditEntry>> GetByInitiatorIdAsync(long initiatorId, CancellationToken ct = default);
    Task<IEnumerable<EventAuditEntry>> GetByTypeAsync(EventType type, CancellationToken ct = default);
    Task<IEnumerable<EventAuditEntry>> GetRecentAsync(int limit = 50, CancellationToken ct = default);
}
