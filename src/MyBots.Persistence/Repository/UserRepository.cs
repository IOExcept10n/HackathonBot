using Microsoft.EntityFrameworkCore;
using MyBots.Core.Persistence.DTO;

namespace MyBots.Core.Persistence.Repository;

/// <summary>
/// Repository for managing user entities.
/// </summary>
public class UserRepository(BasicBotDbContext context) : Repository<BasicBotDbContext, User>(context), IUserRepository
{

    /// <summary>
    /// Gets a user by their Telegram ID.
    /// </summary>
    /// <param name="telegramId">The Telegram user ID.</param>
    /// <param name="ct">An instance of the <see cref="CancellationToken"/> to cancel operation.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User?> GetByTelegramIdAsync(long telegramId, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.TelegramId == telegramId, ct);
    }

    public async Task<User> GetOrCreateByTelegramIdAsync(long telegramId, string username = "", CancellationToken ct = default)
    {
        var user = await GetByTelegramIdAsync(telegramId, ct);
        if (user == null)
        {
            user = new()
            {
                Name = username,
                TelegramId = telegramId
            };
            await AddAsync(user, ct);
            await SaveChangesAsync(ct);
        }
        return user;
    }
}