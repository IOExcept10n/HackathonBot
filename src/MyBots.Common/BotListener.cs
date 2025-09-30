using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyBots.Core.Fsm;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyBots.Core
{
    public class BotListener(IFsmDispatcher dispatcher, IOptions<BotStartupConfig> config, ILogger<BotListener> logger)
    {
        private readonly IFsmDispatcher _dispatcher = dispatcher;
        private readonly BotStartupConfig _config = config.Value;
        private readonly ILogger _logger = logger;

        private int _running;
        private TelegramBotClient? _client;
        private CancellationTokenSource? _cts;

        public async Task Run()
        {
            if (Interlocked.CompareExchange(ref _running, 1, 0) != 0)
                throw new InvalidOperationException("BotEngine is already running.");

            _cts = new CancellationTokenSource();
            try
            {
                _client = new TelegramBotClient(_config.Token, cancellationToken: _cts.Token);
                _dispatcher.Configure(_client);
                _client.OnUpdate += OnUpdateReceived;

                var me = await _client.GetMe(_cts.Token);

                var isInteractive = Console.IsInputRedirected == false && Console.IsOutputRedirected == false && IsConsoleAttached();
                if (isInteractive)
                {
                    _logger.LogInformation("@{Username} is running... Press Escape to terminate", me.Username);
                    while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Escape)
                    {
                        await Task.Delay(200, _cts.Token).ContinueWith(_ => { }, TaskScheduler.Default);
                    }

                    _cts.Cancel();
                }
                else
                {
                    _logger.LogInformation("@{Username} is running (non-interactive). Waiting for stop signal...", me.Username);
                    try
                    {
                        await Task.Delay(Timeout.Infinite, _cts.Token);
                    }
                    catch (TaskCanceledException) { /* expected */ }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BotListener crashed");
                throw;
            }
            finally
            {
                try
                {
                    if (_client != null)
                    {
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
                _logger.LogInformation("Stop requested");
                _cts?.Cancel();
            }
        }

        private async Task OnUpdateReceived(Update update)
        {
            try
            {
                await _dispatcher.HandleUpdateAsync(update, _cts!.Token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling update");
            }
        }

        private static bool IsConsoleAttached()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return GetConsoleWindow() != IntPtr.Zero;
                else
                    return File.Exists("/dev/tty");
            }
            catch { return false; }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
    }
}
