using MyBots.Core.Persistence.DTO;

namespace MyBots.Core.Fsm.States;

public interface IUserStateService
{
    Task<StateDefinition> GetUserRootStateAsync(User user);
}
