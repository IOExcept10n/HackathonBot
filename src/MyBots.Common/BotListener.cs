using System.Runtime.InteropServices;
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
            if (Interlocked.CompareExchange(ref _running, 1, 0) != 0)
                throw new InvalidOperationException("BotEngine is already running.");

            _cts = new CancellationTokenSource();
            try
            {
                _client = new TelegramBotClient(_config.Token, cancellationToken: _cts.Token);
                _dispatcher.Configure(_client);
                _client.OnUpdate += OnUpdateReceived;

                var me = await _client.GetMe(_cts.Token);

                // Detect non-interactive environment (no TTY) and behave accordingly.
                var isInteractive = Console.IsInputRedirected == false && Console.IsOutputRedirected == false && IsConsoleAttached();
                if (isInteractive)
                {
                    Console.WriteLine($"@{me.Username} is running... Press Escape to terminate");
                    // wait for Escape or cancellation
                    while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Escape)
                    {
                        await Task.Delay(200, _cts.Token).ContinueWith(_ => { }, TaskScheduler.Default);
                    }

                    _cts.Cancel();
                }
                else
                {
                    // Non-interactive: just log and wait until cancellation is requested.
                    Console.WriteLine($"@{me.Username} is running (non-interactive). Waiting for stop signal...");
                    try
                    {
                        await Task.Delay(Timeout.Infinite, _cts.Token);
                    }
                    catch (TaskCanceledException) { /* expected on stop */ }
                }
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
                _cts?.Cancel();
        }

        private async Task OnUpdateReceived(Update update) => await _dispatcher.HandleUpdateAsync(update, _cts!.Token);

        // Best-effort check whether a real console is attached (works cross-platform).
        private static bool IsConsoleAttached()
        {
            try
            {
                // Console.OpenStandardInput/Output succeed even when redirected; use P/Invoke on Windows and /dev/tty on *nix.
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // On Windows, GetConsoleWindow==0 means no console.
                    return GetConsoleWindow() != IntPtr.Zero;
                }
                else
                {
                    // On Unix, check if /dev/tty can be opened for reading.
                    return File.Exists("/dev/tty");
                }
            }
            catch
            {
                return false;
            }
        }

        // P/Invoke for Windows console detection
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
    }
}
