using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBots.Core.Fsm.Persistency;

public interface IStateStore
{
    Task<SessionState?> GetAsync(long userId, CancellationToken ct = default);
    Task SaveAsync(SessionState state, CancellationToken ct = default);
    Task DeleteAsync(long userId, CancellationToken ct = default);
}