using HackathonBot.Models.Kmm;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Repository;

internal class AbilityUseRepository(BotDbContext ctx) : BotRepository<AbilityUse>(ctx), IAbilityUseRepository
{
    public async Task<IEnumerable<AbilityUse>> GetByTeamIdAsync(long teamId, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .Where(a => a.TeamId == teamId)
            .OrderBy(a => a.UsedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<AbilityUse>> GetByAbilityAsync(Ability ability, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .Where(a => a.Ability == ability)
            .OrderByDescending(a => a.UsedAt)
            .ToListAsync(ct);

    public async Task<AbilityUse?> GetLastUseAsync(long teamId, Ability ability, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .Where(a => a.TeamId == teamId && a.Ability == ability)
            .OrderByDescending(a => a.UsedAt)
            .FirstOrDefaultAsync(ct);
}
