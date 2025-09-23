namespace MyBots.Core.Localization;

/// <summary>
/// Configuration options for the localization system.
/// </summary>
public class LocalizationOptions
{
    /// <summary>
    /// Gets or sets the path to the directory containing resource files.
    /// </summary>
    public string ResourcesPath { get; set; } = "Resources";

    /// <summary>
    /// Gets or sets the base name for resource files.
    /// </summary>
    public string ResourceBaseName { get; set; } = "Strings";

    /// <summary>
    /// Gets or sets the default language code.
    /// </summary>
    public string DefaultLanguage { get; set; } = "en-US";
}