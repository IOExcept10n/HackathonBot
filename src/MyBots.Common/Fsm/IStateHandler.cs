namespace MyBots.Core.Fsm;

/// <summary>
/// Defines a handler for a specific FSM state that processes user input and determines the next state.
/// </summary>
public interface IStateHandler
{
    /// <summary>
    /// Handles an incoming update in the current state and determines the next state and actions.
    /// </summary>
    /// <param name="ctx">The context containing user input, session data, and available services.</param>
    /// <returns>A result indicating the next state and actions to perform.</returns>
    Task<StateResult> HandleAsync(StateContext ctx);
}