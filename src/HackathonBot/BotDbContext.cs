using Microsoft.EntityFrameworkCore;
using MyBots.Core.Persistence;

namespace HackathonBot;

internal class BotDbContext(DbContextOptions<BotDbContext> options) : BasicBotDbContext(options)
{
}