using System.Diagnostics.CodeAnalysis;
using MyBots.Core.Fsm.States;

namespace MyBots.Modules.Common.Handling;

public class StateHandlerRegistry(IStateRegistry stateRegistry) : IStateHandlerRegistry
{
    private readonly IStateRegistry _stateRegistry = stateRegistry;
    private readonly Dictionary<StateDefinition, IStateHandler> _handlers = [];

    public void Register(StateDefinition state, IStateHandler handler)
    {
        _stateRegistry.Register(state);
        _handlers.Add(state, handler);
    }

    public bool TryGet(StateDefinition state, [NotNullWhen(true)] out IStateHandler? handler)
        => _handlers.TryGetValue(state, out handler);
}
