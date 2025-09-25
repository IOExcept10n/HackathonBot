using MyBots.Modules.Common.Roles;

namespace HackathonBot.Services
{
    internal class RoleProvider : IRoleProvider
    {
        public Task<Role> GetRoleAsync(long userId, CancellationToken cancellationToken = default) => Task.FromResult(Roles.User);
    }
}