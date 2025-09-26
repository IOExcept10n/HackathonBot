namespace HackathonBot.Repository;

internal class BotRepository<T>(BotDbContext ctx) : MyBots.Core.Persistence.Repository.Repository<BotDbContext, T>(ctx)
    where T : class
{
}