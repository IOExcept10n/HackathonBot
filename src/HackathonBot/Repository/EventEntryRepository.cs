using HackathonBot.Models.Kmm;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Repository;

internal class EventEntryRepository(BotDbContext ctx) : BotRepository<EventEntry>(ctx), IEventEntryRepository
{
    public async Task<IEnumerable<EventEntry>> GetByQuestIdAsync(long questId, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .Where(en => en.QuestId == questId)
            .Include(en => en.Team)
            .Include(en => en.Quest)
            .ToListAsync(ct);

    public async Task<IEnumerable<EventEntry>> GetByTeamIdAsync(long teamId, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .Where(en => en.KmmTeamId == teamId)
            .Include(en => en.Quest)
            .ToListAsync(ct);

    public async Task<EventEntry?> GetEntryAsync(long questId, long teamId, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(en => en.QuestId == questId && en.KmmTeamId == teamId, ct);
}
