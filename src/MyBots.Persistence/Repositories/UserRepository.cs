using Microsoft.EntityFrameworkCore;
using MyBots.Persistence.DTO;
using MyBots.Persistence.Repository;

namespace MyBots.Persistence.Repositories;

/// <summary>
/// Repository for managing user entities.
/// </summary>
public class UserRepository : Repository<BasicBotDbContext, User>
{
    public UserRepository(BasicBotDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets a user by their Telegram ID.
    /// </summary>
    /// <param name="telegramId">The Telegram user ID.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User?> GetByTelegramIdAsync(long telegramId)
    {
        return await _dbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.TelegramId == telegramId);
    }

    /// <summary>
    /// Gets all users with a specific role.
    /// </summary>
    /// <param name="roleId">The ID of the role to filter by.</param>
    /// <returns>A collection of users with the specified role.</returns>
    public async Task<IEnumerable<User>> GetByRoleAsync(int roleId)
    {
        return await _dbSet
            .Include(u => u.Role)
            .Where(u => u.Role != null && u.Role.Id == roleId)
            .ToListAsync();
    }
}