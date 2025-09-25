using System.Diagnostics.CodeAnalysis;
using MyBots.Core.Fsm.States;

namespace MyBots.Modules.Common.Handling;

public interface IStateHandlerRegistry
{
    void Register(StateDefinition state, IStateHandler handler);

    bool TryGet(StateDefinition state, [NotNullWhen(true)] out IStateHandler? handler);
}
