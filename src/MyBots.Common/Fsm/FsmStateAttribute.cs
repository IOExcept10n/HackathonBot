using MyBots.Core.Models;

namespace MyBots.Core.Fsm;

/// <summary>
/// Marks a class as an FSM state handler and provides configuration for the state.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class FsmStateAttribute(string stateId) : Attribute
{
    /// <summary>
    /// Gets the unique identifier for the FSM state.
    /// </summary>
    public string StateId { get; } = stateId;

    /// <summary>
    /// Gets or sets the module this state belongs to. Defaults to "default".
    /// </summary>
    public string Module { get; init; } = "default";

    /// <summary>
    /// Gets or sets the roles allowed to access this state.
    /// </summary>
    public Role[] AllowedRoles { get; init; } = [];

    /// <summary>
    /// Gets or sets whether this state is the initial state for the specified roles.
    /// </summary>
    public bool IsRootForRole { get; init; } = false;

    /// <summary>
    /// Gets or sets whether this state handles user input prompts.
    /// </summary>
    public bool IsPromptState { get; init; } = false;
}