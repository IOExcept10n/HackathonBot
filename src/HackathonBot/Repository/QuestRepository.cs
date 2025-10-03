using HackathonBot.Models.Kmm;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Repository;

internal class QuestRepository(BotDbContext ctx) : BotRepository<Quest>(ctx), IQuestRepository
{
    public async Task<Quest?> FindByNameAsync(long eventId, string name, CancellationToken ct = default) => 
        await _dbSet
            .Where(q => q.EventId == eventId && !q.IsSabotage)
            .FirstOrDefaultAsync(q => q.Name == name, ct);

    public async Task<IEnumerable<Quest>> GetByEventIdAsync(long eventId, CancellationToken ct = default) =>
        await _dbSet
            .Where(q => q.EventId == eventId && !q.IsSabotage)
            .ToListAsync(ct);

    public async Task<IEnumerable<Quest>> GetSabotagesByEventIdAsync(long eventId, CancellationToken ct = default) =>
        await _dbSet
            .Where(q => q.EventId == eventId && q.IsSabotage)
            .ToListAsync(ct);
}