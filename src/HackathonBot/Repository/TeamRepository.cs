using HackathonBot.Models;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Repository;

internal class TeamRepository(BotDbContext ctx) : BotRepository<Team>(ctx), ITeamRepository
{
    public async Task<Team?> GetWithMembersAsync(Guid teamId, CancellationToken ct = default) =>
        await _dbSet.Include(t => t.Members).FirstOrDefaultAsync(t => t.Id == teamId, ct);

    public async Task<Team?> FindByNameAsync(string name, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Name == name, ct);
}
