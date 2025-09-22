namespace MyBots.Core.Fsm;

public class StateResult
{
    public string? NextStateId { get; init; }
    public string? NextStateDataJson { get; init; }
    public IEnumerable<BotAction> Actions { get; init; } = [];
    public bool KeepHistory { get; init; } = true;
    public static StateResult Stay(params BotAction[] actions) => new() { Actions = actions };
    public static StateResult Transition(string next, string? dataJson = null, params BotAction[] actions) =>
        new() { NextStateId = next, NextStateDataJson = dataJson, Actions = actions };
}