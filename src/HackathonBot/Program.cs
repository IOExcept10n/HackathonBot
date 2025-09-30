using System.Text;
using HackathonBot;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();                 // пишет в stdout/stderr -> docker logs
        logging.SetMinimumLevel(LogLevel.Information);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHackathonBot(context.Configuration);
        // регистрация BotListener и т.п.
        services.AddSingleton<BotListener>();
    })
    .Build();


#if RELEASE
host.Services.ConfigureMigrations();
#endif

await host.Services.ConfigureCreatorAsync();

BotListener listener = host.Services.GetRequiredService<BotListener>();
await listener.Run();
