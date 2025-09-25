using System.Diagnostics.CodeAnalysis;

namespace MyBots.Core.Fsm.States;

public interface IStateHandlerProvider
{
    bool TryGetHandler(StateDefinition state, [NotNullWhen(true)] out IStateHandler? handler);
}
