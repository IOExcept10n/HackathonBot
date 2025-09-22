using System.Globalization;
using System.Resources;

namespace MyBots.Core.Localization;

public class LocalizationService(ResourceManager resourceManager, string defaultLanguage = "en-US") : ILocalizationService
{
    private readonly Dictionary<long, CultureInfo> _userLanguages = [];
    private readonly ResourceManager _resourceManager = resourceManager;
    private readonly string _defaultLanguage = defaultLanguage;

    public Task SetUserLanguageAsync(long userId, CultureInfo culture)
    {
        _userLanguages[userId] = culture;
        return Task.CompletedTask;
    }

    public Task<CultureInfo> GetUserLanguageAsync(long userId)
    {
        if (_userLanguages.TryGetValue(userId, out var culture))
            return Task.FromResult(culture);
        return Task.FromResult(new CultureInfo(_defaultLanguage));
    }

    public async Task<string> GetStringAsync(string key, long userId)
    {
        var culture = await GetUserLanguageAsync(userId);
        return _resourceManager.GetString(key, culture) ?? key;
    }

    public async Task<string> GetStringAsync(string key, long userId, params object[] args)
    {
        var format = await GetStringAsync(key, userId);
        return string.Format(format, args);
    }
}