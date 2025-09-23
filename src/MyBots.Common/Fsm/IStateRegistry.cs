using System.Diagnostics.CodeAnalysis;
using MyBots.Core.Models;

namespace MyBots.Core.Fsm;

/// <summary>
/// Represents the definition of a state in the FSM system.
/// </summary>
/// <param name="StateId">The unique identifier of the state.</param>
/// <param name="Module">The module this state belongs to.</param>
/// <param name="HandlerType">The type of the handler that processes this state.</param>
/// <param name="AllowedRoles">The roles that are allowed to access this state.</param>
/// <param name="IsRootForRole">Whether this state is the initial state for the specified roles.</param>
/// <param name="IsPromptState">Whether this state handles user input prompts.</param>
public record StateDefinition(string StateId, string Module, Type HandlerType, Role[] AllowedRoles, bool IsRootForRole, bool IsPromptState);

/// <summary>
/// Provides registration and lookup of FSM states and their handlers.
/// </summary>
public interface IStateRegistry
{
    /// <summary>
    /// Registers a new state definition in the registry.
    /// </summary>
    /// <param name="def">The state definition to register.</param>
    void Register(StateDefinition def);

    /// <summary>
    /// Attempts to retrieve a state definition by its ID.
    /// </summary>
    /// <param name="stateId">The ID of the state to retrieve.</param>
    /// <param name="def">When successful, contains the state definition; otherwise, null.</param>
    /// <returns>True if the state was found; otherwise, false.</returns>
    bool TryGet(string stateId, [NotNullWhen(true)] out StateDefinition? def);

    /// <summary>
    /// Lists all state definitions accessible to the specified role.
    /// </summary>
    /// <param name="role">The role to check access for.</param>
    /// <returns>A collection of state definitions accessible to the role.</returns>
    IEnumerable<StateDefinition> ListForRole(Role role);

    /// <summary>
    /// Enables or disables a module in the system.
    /// </summary>
    /// <param name="moduleId">The ID of the module to enable or disable.</param>
    /// <param name="enabled">True to enable the module; false to disable it.</param>
    void EnableModule(string moduleId, bool enabled);

    /// <summary>
    /// Checks if a module is currently enabled.
    /// </summary>
    /// <param name="moduleId">The ID of the module to check.</param>
    /// <returns>True if the module is enabled; otherwise, false.</returns>
    bool IsModuleEnabled(string moduleId);
}