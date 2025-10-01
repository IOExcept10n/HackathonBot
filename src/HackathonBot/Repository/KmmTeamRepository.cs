using HackathonBot.Models.Kmm;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Repository;

internal class KmmTeamRepository(BotDbContext ctx) : BotRepository<KmmTeam>(ctx), IKmmTeamRepository
{
    public async Task<KmmTeam?> GetByIdWithLogsAsync(long id, CancellationToken ct = default) =>
        await _dbSet
            .Include(t => t.AbilitiesLog)
            .Include(t => t.EventEntries)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<IEnumerable<KmmTeam>> GetAliveTeamsAsync(CancellationToken ct = default) =>
        await _dbSet
            .Where(t => t.IsAlive)
            .ToListAsync(ct);
}
