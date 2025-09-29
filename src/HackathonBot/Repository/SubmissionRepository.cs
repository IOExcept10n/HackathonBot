using HackathonBot.Models;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Repository;

internal class SubmissionRepository(BotDbContext ctx) : BotRepository<Submission>(ctx), ISubmissionRepository
{
    public async Task<Submission?> GetByTeamIdAsync(Guid teamId, CancellationToken ct = default) =>
        await _dbSet
            .FirstOrDefaultAsync(s => s.TeamId == teamId, ct);

    public async Task<IList<Submission>> GetByCaseAsync(Case c, CancellationToken ct = default) =>
        await _dbSet
            .Where(s => s.Case == c)
            .ToListAsync(ct);
}
