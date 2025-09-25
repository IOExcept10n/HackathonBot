using Microsoft.Extensions.DependencyInjection;
using MyBots.Core.Fsm;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyBots.Core
{
    public class BotListener(IServiceProvider services, BotStartupConfig config)
    {
        private readonly IFsmDispatcher _dispatcher = services.GetRequiredService<IFsmDispatcher>();
        private readonly BotStartupConfig _config = config;

        private int _running;

        private TelegramBotClient? _client;
        private CancellationTokenSource? _cts;

        public async Task Run()
        {
            // Try to set _running from 0 to 1 atomically. If it was already 1, throw.
            if (Interlocked.CompareExchange(ref _running, 1, 0) != 0)
            {
                throw new InvalidOperationException("BotEngine is already running.");
            }

            _cts = new CancellationTokenSource();
            try
            {
                _client = new TelegramBotClient(_config.Token, cancellationToken: _cts.Token);
                _dispatcher.Configure(_client);
                _client.OnUpdate += OnUpdateReceived;

                var me = await _client.GetMe(_cts.Token);
                Console.WriteLine($"@{me.Username} is running... Press Escape to terminate");

                // wait for Escape or cancellation
                while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Escape)
                {
                    await Task.Delay(200, _cts.Token).ContinueWith(_ => { }, TaskScheduler.Default);
                }

                // Cancel and wait a moment for handlers to finish (optional)
                _cts.Cancel();
            }
            finally
            {
                // cleanup and mark as stopped
                try
                {
                    if (_client != null)
                    {
                        // detach handlers to avoid leaks
                        _client.OnUpdate -= OnUpdateReceived;
                        _client = null;
                    }
                    _cts?.Dispose();
                    _cts = null;
                }
                finally
                {
                    Interlocked.Exchange(ref _running, 0);
                }
            }
        }

        public bool IsRunning => Volatile.Read(ref _running) == 1;

        public void Stop()
        {
            if (Volatile.Read(ref _running) == 1)
            {
                _cts?.Cancel();
            }
        }

        private async Task OnUpdateReceived(Update update) => await _dispatcher.HandleUpdateAsync(update, _cts!.Token);
    }
}
