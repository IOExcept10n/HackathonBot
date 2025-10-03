using HackathonBot.Models.Kmm;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Repository;

internal class EventAuditRepository(BotDbContext ctx) : BotRepository<EventAuditEntry>(ctx), IEventAuditRepository
{
    public async Task<IEnumerable<EventAuditEntry>> GetByInitiatorIdAsync(long initiatorId, CancellationToken ct = default)
        => await _dbSet.AsNoTracking()
            .Where(e => e.InitiatorId == initiatorId)
            .Include(e => e.Initiator)
            .OrderByDescending(e => e.LoggedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<EventAuditEntry>> GetByTypeAsync(EventType type, CancellationToken ct = default)
        => await _dbSet.AsNoTracking()
            .Where(e => e.EventType == type)
            .Include(e => e.Initiator)
            .OrderByDescending(e => e.LoggedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<EventAuditEntry>> GetRecentAsync(int limit = 50, CancellationToken ct = default)
        => await _dbSet.AsNoTracking()
        .Include(e => e.Initiator)
        .OrderByDescending(e => e.LoggedAt)
        .Take(limit)
        .ToListAsync(ct);
}
