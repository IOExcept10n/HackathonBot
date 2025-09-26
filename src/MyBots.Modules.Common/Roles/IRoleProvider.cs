using MyBots.Core.Persistence.DTO;

namespace MyBots.Modules.Common.Roles;

public interface IRoleProvider
{
    Task<Role> GetRoleAsync(User user, CancellationToken cancellationToken = default);
}