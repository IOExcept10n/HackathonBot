using MyBots.Core.Persistence.DTO;

namespace MyBots.Core.Persistence.Repository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByTelegramIdAsync(long telegramId, CancellationToken ct = default);
    Task<User> GetOrCreateByTelegramIdAsync(long telegramId, string username = "", CancellationToken ct = default);
}