using System.Globalization;
using System.Resources;

namespace MyBots.Core.Localization;

/// <summary>
/// Implements the localization service using a resource manager to provide localized strings.
/// </summary>
/// <remarks>
/// This implementation stores user language preferences in memory and uses a resource manager
/// to fetch localized strings from embedded resources.
/// </remarks>
/// <param name="resourceManager">The resource manager used to fetch localized strings.</param>
/// <param name="defaultLanguage">The default language code to use when no user preference is set. Defaults to "en-US".</param>
public class LocalizationService(ResourceManager resourceManager, string defaultLanguage = "en-US") : ILocalizationService
{
    private readonly Dictionary<long, CultureInfo> _userLanguages = [];
    private readonly ResourceManager _resourceManager = resourceManager;
    private readonly string _defaultLanguage = defaultLanguage;

    /// <inheritdoc />
    public Task SetUserLanguageAsync(long userId, CultureInfo culture)
    {
        _userLanguages[userId] = culture;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<CultureInfo> GetUserLanguageAsync(long userId)
    {
        if (_userLanguages.TryGetValue(userId, out var culture))
            return Task.FromResult(culture);
        return Task.FromResult(new CultureInfo(_defaultLanguage));
    }

    /// <inheritdoc />
    public async Task<string> GetStringAsync(string key, long userId)
    {
        var culture = await GetUserLanguageAsync(userId);
        return _resourceManager.GetString(key, culture) ?? key;
    }

    /// <inheritdoc />
    public async Task<string> GetStringAsync(string key, long userId, params object[] args)
    {
        var format = await GetStringAsync(key, userId);
        return string.Format(format, args);
    }
}