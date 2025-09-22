using System.Globalization;

namespace MyBots.Core.Localization;

/// <summary>
/// Provides methods for retrieving localized strings and managing a user's language preference.
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// Asynchronously retrieves a localized string for the specified key and user.
    /// </summary>
    /// <param name="key">The resource key identifying the localized string. Cannot be null or empty.</param>
    /// <param name="userId">The identifier of the user for whom the localization should be resolved.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
    /// The task result contains the localized string. If the key is not found a fallback or the key itself may be returned depending on implementation.
    /// </returns>
    Task<string> GetStringAsync(string key, long userId);

    /// <summary>
    /// Asynchronously retrieves a formatted localized string for the specified key and user, using the provided arguments.
    /// </summary>
    /// <param name="key">The resource key identifying the localized string. Cannot be null or empty.</param>
    /// <param name="userId">The identifier of the user for whom the localization should be resolved.</param>
    /// <param name="args">Optional format arguments to be applied to the localized string (as in <c>string.Format</c>).</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
    /// The task result contains the formatted localized string. If the key is not found a fallback or the key itself may be returned depending on implementation.
    /// </returns>
    Task<string> GetStringAsync(string key, long userId, params object[] args);

    /// <summary>
    /// Asynchronously sets the preferred <see cref="CultureInfo"/> for the specified user.
    /// </summary>
    /// <param name="userId">The identifier of the user whose language preference will be set.</param>
    /// <param name="culture">The <see cref="CultureInfo"/> to assign to the user. If null, implementations may clear the preference or use a default culture.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task SetUserLanguageAsync(long userId, CultureInfo culture);

    /// <summary>
    /// Asynchronously retrieves the preferred <see cref="CultureInfo"/> for the specified user.
    /// </summary>
    /// <param name="userId">The identifier of the user whose language preference will be retrieved.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
    /// The task result contains the user's preferred <see cref="CultureInfo"/>. If no preference is set, implementations should return a default culture (e.g., <see cref="CultureInfo.CurrentCulture"/> or a configured fallback).
    /// </returns>
    Task<CultureInfo> GetUserLanguageAsync(long userId);
}
