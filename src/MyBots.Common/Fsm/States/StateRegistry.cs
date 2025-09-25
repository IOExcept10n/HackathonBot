using System.Diagnostics.CodeAnalysis;

namespace MyBots.Core.Fsm.States;

public class StateRegistry : IStateRegistry
{
    public const string InitialStateName = "start";

    private readonly Dictionary<string, StateDefinition> _registry = [];

    public StateDefinition GetInitialState() => _registry[InitialStateName];

    public void Register(StateDefinition def)
    {
        _registry.Add(def.StateId, def);
    }

    public bool TryGet(string stateId, [NotNullWhen(true)] out StateDefinition? def)
        => _registry.TryGetValue(stateId, out def);
}
