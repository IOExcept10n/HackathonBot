using Microsoft.EntityFrameworkCore;
using MyBots.Persistence.DTO;
using MyBots.Persistence.Repository;

namespace MyBots.Persistence.Repositories;

/// <summary>
/// Repository for managing audit log entries.
/// </summary>
public class AuditLogRepository : Repository<BasicBotDbContext, AuditLog>
{
    public AuditLogRepository(BasicBotDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets audit log entries for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="limit">The maximum number of entries to return.</param>
    /// <param name="offset">The number of entries to skip.</param>
    /// <returns>A collection of audit log entries.</returns>
    public async Task<IEnumerable<AuditLog>> GetUserLogsAsync(long userId, int limit = 100, int offset = 0)
    {
        return await _dbSet
            .Include(l => l.User)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    /// <summary>
    /// Gets audit log entries for a specific module.
    /// </summary>
    /// <param name="moduleName">The name of the module.</param>
    /// <param name="limit">The maximum number of entries to return.</param>
    /// <param name="offset">The number of entries to skip.</param>
    /// <returns>A collection of audit log entries.</returns>
    public async Task<IEnumerable<AuditLog>> GetModuleLogsAsync(string moduleName, int limit = 100, int offset = 0)
    {
        return await _dbSet
            .Include(l => l.User)
            .Where(l => l.ModuleName == moduleName)
            .OrderByDescending(l => l.Timestamp)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    /// <summary>
    /// Records a new action in the audit log.
    /// </summary>
    /// <param name="userId">The ID of the user who performed the action.</param>
    /// <param name="action">The action that was performed.</param>
    /// <param name="moduleName">The module where the action was performed.</param>
    /// <param name="details">Additional details about the action.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task LogActionAsync(long userId, string action, string moduleName, string details = "")
    {
        var log = new AuditLog
        {
            UserId = userId,
            Action = action,
            ModuleName = moduleName,
            Details = details,
            Timestamp = DateTime.UtcNow
        };

        await AddAsync(log);
        await SaveChangesAsync();
    }
}