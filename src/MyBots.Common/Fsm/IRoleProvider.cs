using MyBots.Core.Models;

namespace MyBots.Core.Fsm;

public interface IRoleProvider
{
    Task<Role> GetRoleAsync(long userId, CancellationToken ct = default);
}