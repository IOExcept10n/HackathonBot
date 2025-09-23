using System.Collections.Concurrent;
using System.Resources;
using System.Text.Json;

namespace MyBots.Core.Localization;

/// <summary>
/// Manages loading and accessing localized resources from JSON files.
/// </summary>
public class JsonResourceManager : ResourceManager
{
    private readonly ConcurrentDictionary<string, Dictionary<string, string>> _resources = new();
    private readonly string _resourcesPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonResourceManager"/> class.
    /// </summary>
    /// <param name="baseName">The base name of the resource files (e.g., "Strings").</param>
    /// <param name="resourcesPath">The path to the directory containing resource files.</param>
    public JsonResourceManager(string baseName, string resourcesPath) : base(baseName, null)
    {
        _resourcesPath = resourcesPath;
    }

    /// <summary>
    /// Gets the file path for a specific culture.
    /// </summary>
    /// <param name="culture">The culture code (e.g., "en-US").</param>
    /// <returns>The path to the resource file.</returns>
    private string GetResourcePath(string culture)
    {
        return Path.Combine(_resourcesPath, $"{BaseName}.{culture}.json");
    }

    /// <summary>
    /// Loads resources for a specific culture from a JSON file.
    /// </summary>
    /// <param name="culture">The culture code (e.g., "en-US").</param>
    /// <returns>True if resources were loaded successfully; otherwise, false.</returns>
    public bool LoadResources(string culture)
    {
        var path = GetResourcePath(culture);
        if (!File.Exists(path))
            return false;

        try
        {
            var json = File.ReadAllText(path);
            var resources = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (resources != null)
            {
                _resources[culture] = resources;
                return true;
            }
        }
        catch (Exception)
        {
            // Log the error or handle it appropriately
        }

        return false;
    }

    /// <inheritdoc />
    public override string GetString(string name, System.Globalization.CultureInfo culture)
    {
        var cultureName = culture.Name;

        if (!_resources.ContainsKey(cultureName))
        {
            LoadResources(cultureName);
        }

        if (_resources.TryGetValue(cultureName, out var resources) &&
            resources.TryGetValue(name, out var value))
        {
            return value;
        }

        // If not found in specific culture, try neutral culture
        var neutralCulture = culture.Parent.Name;
        if (!string.IsNullOrEmpty(neutralCulture) &&
            !_resources.ContainsKey(neutralCulture))
        {
            LoadResources(neutralCulture);
        }

        if (!string.IsNullOrEmpty(neutralCulture) &&
            _resources.TryGetValue(neutralCulture, out resources) &&
            resources.TryGetValue(name, out value))
        {
            return value;
        }

        // If not found, try default culture (en-US)
        if (cultureName != "en-US" && !_resources.ContainsKey("en-US"))
        {
            LoadResources("en-US");
        }

        if (cultureName != "en-US" &&
            _resources.TryGetValue("en-US", out resources) &&
            resources.TryGetValue(name, out value))
        {
            return value;
        }

        return name;
    }
}