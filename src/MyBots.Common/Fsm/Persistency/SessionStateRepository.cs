using MyBots.Persistence.Repository;

namespace MyBots.Core.Fsm.Persistency
{
    public class SessionStateRepository(FsmDbContextBase context) : Repository<FsmDbContextBase, SessionState>(context)
    {
    }
}