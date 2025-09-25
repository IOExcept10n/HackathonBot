using System.Diagnostics.CodeAnalysis;

namespace MyBots.Core.Fsm.States
{
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
    }
}
