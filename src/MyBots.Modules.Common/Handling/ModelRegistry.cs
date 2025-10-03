using System.Reflection;
using System.ComponentModel.DataAnnotations;
using MyBots.Core.Localization;
using System.Diagnostics.CodeAnalysis;

namespace MyBots.Modules.Common.Handling;

internal class ModelRegistry(ILocalizationService localization) : IModelRegistry
{
    private readonly ILocalizationService _localization = localization;
    private readonly Dictionary<(string, string), ModelBindingDescription> descriptions = [];

    public void Register(string moduleName, Type modelType)
    {
        var key = (moduleName, modelType.FullName ?? modelType.Name);
        if (!descriptions.TryGetValue(key, out var description))
        {
            description = new(
                moduleName,
                modelType,
                [..from p in modelType.GetProperties()
                   where p.CanWrite
                   let display = p.GetCustomAttribute<DisplayAttribute>()
                   select new ModelProperty(p.Name, _localization.GetString(display?.Prompt ?? p.Name), p)]);
            descriptions.Add(key, description);
        }
    }

    public bool TryGetDescription(string moduleName, string typeName, [NotNullWhen(true)] out ModelBindingDescription? description)
        => descriptions.TryGetValue((moduleName, typeName), out description);
}