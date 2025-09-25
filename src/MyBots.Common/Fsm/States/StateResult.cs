namespace MyBots.Core.Fsm.States;

public record StateResult(string NextStateId, string NextStateData, string? OverrideNextStateMessage = null);
