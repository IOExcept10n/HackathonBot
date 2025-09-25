using HackathonBot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBots.Core;

IConfiguration config = new ConfigurationBuilder()
#if DEBUG
    .AddJsonFile("appsettings.Development.json")
#else
    .AddJsonFile("appsettings.json")
#endif
    .AddJsonFile("appsettings.Secrets.json")
    .Build();

IServiceProvider services = new ServiceCollection()
    .AddHackathonBot(config)
    .BuildServiceProvider();

BotListener listener = new(services, config.GetSection("BotStartupConfig").Get<BotStartupConfig>()!);
await listener.Run();
