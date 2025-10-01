using HackathonBot.Models;
using HackathonBot.Models.Kmm;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Repository;

internal class BankRepository(BotDbContext ctx) : BotRepository<Bank>(ctx), IBankRepository
{
    public async Task<int> GetParticipantCoinsAsync(CancellationToken ct = default) =>
        await GetCoinsAsync(nameof(Participant), ct);

    public async Task SetParticipantCoinsAsync(int value, CancellationToken ct = default) =>
        await SetCoinsAsync(nameof(Participant), value, ct);

    public async Task<int> GetCoinsAsync(string key, CancellationToken ct = default) =>
        (await GetOrCreateBankAsync(key, ct)).Value;

    public async Task SetCoinsAsync(string key, int value, CancellationToken ct = default)
    {
        var bank = await GetOrCreateBankAsync(key, ct);
        bank.Value = value;
        await SaveChangesAsync(ct);
    }

    public async Task<Bank?> FindByKeyAsync(string key, CancellationToken ct = default) =>
        await _dbSet.FirstOrDefaultAsync(b => b.Key == key, ct);

    private async Task<Bank> GetOrCreateBankAsync(string key, CancellationToken ct = default)
    {
        var bank = await FindByKeyAsync(key, ct);
        if (bank == null)
        {
            bank = new() { Key = key, Value = 0 };
            await AddAsync(bank, ct);
            await SaveChangesAsync(ct);
        }
        return bank;
    }
}