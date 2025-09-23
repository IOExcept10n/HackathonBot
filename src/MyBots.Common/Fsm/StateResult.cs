namespace MyBots.Core.Fsm;

/// <summary>
/// Represents the result of state processing, including the next state and actions to perform.
/// </summary>
public class StateResult
{
    /// <summary>
    /// Gets the identifier of the next state. If null, stays in the current state.
    /// </summary>
    public string? NextStateId { get; init; }

    /// <summary>
    /// Gets the serialized data to be passed to the next state.
    /// </summary>
    public string? NextStateDataJson { get; init; }

    /// <summary>
    /// Gets the collection of actions to be performed as a result of state processing.
    /// </summary>
    public IEnumerable<BotAction> Actions { get; init; } = [];

    /// <summary>
    /// Gets a value indicating whether to keep the state in history for navigation.
    /// </summary>
    public bool KeepHistory { get; init; } = true;

    /// <summary>
    /// Creates a result that stays in the current state with specified actions.
    /// </summary>
    /// <param name="actions">The actions to perform while staying in the state.</param>
    /// <returns>A new <see cref="StateResult"/> configured to stay in the current state.</returns>
    public static StateResult Stay(params BotAction[] actions) => new() { Actions = actions };

    /// <summary>
    /// Creates a result that transitions to a new state with optional data and actions.
    /// </summary>
    /// <param name="next">The identifier of the next state.</param>
    /// <param name="dataJson">Optional serialized data to pass to the next state.</param>
    /// <param name="actions">The actions to perform during the transition.</param>
    /// <returns>A new <see cref="StateResult"/> configured for state transition.</returns>
    public static StateResult Transition(string next, string? dataJson = null, params BotAction[] actions) =>
        new() { NextStateId = next, NextStateDataJson = dataJson, Actions = actions };
}