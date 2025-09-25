using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MyBots.Core.Persistence;

namespace HackathonBot
{
    internal class BotDbContextFactory : IDesignTimeDbContextFactory<BotDbContext>
    {
        public BotDbContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BotDbContext>();
            optionsBuilder.UseSqlite(config.GetConnectionString("Sqlite"));
            return new(optionsBuilder.Options);
        }
    }
}
