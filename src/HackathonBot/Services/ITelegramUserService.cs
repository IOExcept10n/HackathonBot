using HackathonBot.Models;

namespace HackathonBot.Services;

public interface ITelegramUserService
{
    Task<long?> GetTelegramIdByUsernameAsync(string username, CancellationToken ct = default);
    Task DeleteUserAsync(string username, CancellationToken ct = default);
    Task<RoleIndex> CheckUserByNameAsync(string username, CancellationToken ct = default);
    Task<BotUserRole?> EnsureRegisteredAsync(long userId, string username, CancellationToken ct = default);
}
