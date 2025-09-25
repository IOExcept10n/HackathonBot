namespace MyBots.Core.Fsm.States;

public record StateDefinition(string Name, string? Module, string ParentStateId, IStateLayout Layout)
{
    public string StateId => Module != null ? $"{Module}:{Name}" : Name;

    public bool IsRoot => string.IsNullOrEmpty(ParentStateId);

    public bool IsSubRoot => ParentStateId == "start";
}
