using System.Text;
using HackathonBot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBots.Core;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

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

#if RELEASE
services.ConfigureMigrations();
#endif

await services.ConfigureCreatorAsync();

BotListener listener = new(services, config.GetSection("BotStartupConfig").Get<BotStartupConfig>()!);
await listener.Run();
