using HackathonBot.Models;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Repository;

internal class BotUserRoleRepository(BotDbContext ctx) : BotRepository<BotUserRole>(ctx), IBotUserRoleRepository
{
    public async Task<BotUserRole?> FindByTelegramIdAsync(long telegramId, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(b => b.TelegramId == telegramId, ct);

    public async Task<BotUserRole?> FindByUsernameAsync(string username, CancellationToken ct = default)
    {
        var canonical = username.AsCanonicalNickname();
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(b => b.Username == canonical, ct);
    }
}
