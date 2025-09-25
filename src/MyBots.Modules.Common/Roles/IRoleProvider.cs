namespace MyBots.Modules.Common.Roles;

public interface IRoleProvider
{
    Task<Role> GetRoleAsync(long userId, CancellationToken cancellationToken = default);
}