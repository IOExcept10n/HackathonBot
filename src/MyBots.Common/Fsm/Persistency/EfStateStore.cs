using Microsoft.EntityFrameworkCore;
using MyBots.Persistence.Repository;

namespace MyBots.Core.Fsm.Persistency;

public class SessionStateStore(IRepository<SessionState> repo) : IStateStore
{
    private readonly IRepository<SessionState> _repo = repo ?? throw new ArgumentNullException(nameof(repo));

    public async Task<SessionState?> GetAsync(long userId, CancellationToken ct = default)
    {
        // Use a no-tracking read to avoid attaching unless caller intends to save.
        return await _repo.GetAll()
                          .AsNoTracking()
                          .FirstOrDefaultAsync(s => s.UserId == userId, ct);
    }

    public async Task SaveAsync(SessionState state, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(state);

        // Read existing record with tracking for update.
        var existing = await _repo.GetAll()
                                  .FirstOrDefaultAsync(s => s.UserId == state.UserId, ct);

        if (existing == null)
        {
            state.UpdatedAt = DateTimeOffset.UtcNow;
            state.Version = Math.Max(1, state.Version);
            await _repo.AddAsync(state, ct);
            await _repo.SaveChangesAsync(ct);
            return;
        }

        // Optimistic concurrency check: incoming Version must match stored Version.
        if (state.Version != existing.Version)
        {
            throw new DbUpdateConcurrencyException(
                $"Concurrency conflict for UserId={state.UserId}. Incoming Version={state.Version}, Current Version={existing.Version}.");
        }

        // Apply updates
        existing.StateId = state.StateId;
        existing.StateDataJson = state.StateDataJson;
        existing.History = state.History;
        existing.UpdatedAt = DateTimeOffset.UtcNow;
        existing.Version++;

        await _repo.UpdateAsync(existing, ct);

        try
        {
            await _repo.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            // Bubble up concurrency exception for caller to decide (retry/abort).
            throw;
        }
    }

    public async Task DeleteAsync(long userId, CancellationToken ct = default)
    {
        var existing = await _repo.GetAll()
                                  .FirstOrDefaultAsync(s => s.UserId == userId, ct);
        if (existing == null) return;

        await _repo.DeleteAsync(existing, ct);
        await _repo.SaveChangesAsync(ct);
    }
}