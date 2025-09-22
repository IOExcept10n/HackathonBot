using System.Diagnostics.CodeAnalysis;
using MyBots.Core.Models;

namespace MyBots.Core.Fsm;

public record StateDefinition(string StateId, string Module, Type HandlerType, Role[] AllowedRoles, bool IsRootForRole, bool IsPromptState);

public interface IStateRegistry
{
    void Register(StateDefinition def);

    bool TryGet(string stateId, [NotNullWhen(true)] out StateDefinition? def);

    IEnumerable<StateDefinition> ListForRole(Role role);

    void EnableModule(string moduleId, bool enabled);

    bool IsModuleEnabled(string moduleId);
}