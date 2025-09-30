using HackathonBot.Models;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Repository;

internal class ParticipantRepository(BotDbContext ctx) : BotRepository<Participant>(ctx), IParticipantRepository
{
    public async Task<Participant?> FindByTelegramIdAsync(long telegramId, CancellationToken ct = default) =>
        await _dbSet
            .Include(x => x.Team)
            .ThenInclude(y => y!.Submission)
            .ThenInclude(z => z!.SubmittedBy)
            .FirstOrDefaultAsync(p => p.TelegramId == telegramId, ct);

    public async Task<Participant?> FindByUsernameAsync(string username, CancellationToken ct = default)
    {
        var canonical = username.AsCanonicalNickname();
        return await _dbSet
            .Include(x => x.Team)
            .ThenInclude(y => y!.Submission)
            .ThenInclude(z => z!.SubmittedBy)
            .FirstOrDefaultAsync(p => p.Nickname == canonical, ct);
    }

    public async Task<IList<Participant>> GetByTeamIdAsync(Guid teamId, CancellationToken ct = default) =>
        await _dbSet
            .Include(x => x.Team)
            .ThenInclude(y => y!.Submission)
            .ThenInclude(z => z!.SubmittedBy)
            .Where(p => p.TeamId == teamId)
            .ToListAsync(ct);

    public async Task<bool> IsLeaderAsync(Guid participantId, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .Where(p => p.Id == participantId)
            .Select(p => p.IsLeader)
            .FirstOrDefaultAsync(ct);

    public async Task<bool> IsLeaderAsync(long telegramId, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .Where(p => p.TelegramId == telegramId)
            .Select(p => p.IsLeader)
            .FirstOrDefaultAsync(ct);
}
