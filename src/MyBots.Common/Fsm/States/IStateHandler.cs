namespace MyBots.Core.Fsm.States;

public interface IStateHandler
{
    Task<StateResult> ExecuteAsync(StateContext ctx, CancellationToken cancellationToken = default);
}