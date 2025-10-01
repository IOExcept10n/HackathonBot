using HackathonBot.Models.Kmm;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Repository;

internal class EventRepository(BotDbContext ctx) : BotRepository<Event>(ctx), IEventRepository
{
    public async Task<Event?> GetByIdWithQuestsAsync(long id, CancellationToken ct = default) =>
        await _dbSet
            .Include(e => e.Quests)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IEnumerable<Event>> GetAllWithQuestsAsync(CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .Include(e => e.Quests)
            .ToListAsync(ct);

    public async Task<Event?> FindByNameAsync(string name, CancellationToken ct = default) =>
        await _dbSet
            .Include(e => e.Quests)
            .FirstOrDefaultAsync(e => e.Name == name, ct);
}
