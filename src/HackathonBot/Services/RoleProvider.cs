using MyBots.Core.Persistence.DTO;
using MyBots.Modules.Common.Roles;

namespace HackathonBot.Services;

internal class RoleProvider(ITelegramUserService userService) : IRoleProvider
{
    private readonly ITelegramUserService _userService = userService;

    public async Task<Role> GetRoleAsync(User user, CancellationToken cancellationToken = default)
    {
        var role = await _userService.EnsureRegisteredAsync(user.TelegramId, user.Name, cancellationToken);
        if (role == null)
            return Role.Unknown;
        return role.Role;
    }
}
