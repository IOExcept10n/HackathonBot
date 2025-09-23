using MyBots.Core.Models;

namespace MyBots.Core.Fsm;

/// <summary>
/// Provides access to user role information for authorization purposes.
/// </summary>
public interface IRoleProvider
{
    /// <summary>
    /// Retrieves the role associated with a user.
    /// </summary>
    /// <param name="userId">The ID of the user to get the role for.</param>
    /// <param name="ct">Optional cancellation token.</param>
    /// <returns>The role assigned to the user.</returns>
    Task<Role> GetRoleAsync(long userId, CancellationToken ct = default);
}