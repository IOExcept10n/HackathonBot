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
public class LocalizationService(ResourceManager resourceManager) : ILocalizationService
{
    private readonly ResourceManager _resourceManager = resourceManager;

    /// <inheritdoc />
    public string GetString(string key) => _resourceManager.GetString(key) ?? key;

    /// <inheritdoc />
    public string GetString(string key, params object[] args)
    {
        var format = GetString(key);
        return string.Format(format, args);
    }
}