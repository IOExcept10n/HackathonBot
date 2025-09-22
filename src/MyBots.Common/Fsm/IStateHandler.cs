namespace MyBots.Core.Fsm;
public interface IStateHandler
{
    Task<StateResult> HandleAsync(StateContext ctx);
}