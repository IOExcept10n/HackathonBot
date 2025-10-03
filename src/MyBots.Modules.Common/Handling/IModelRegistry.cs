using System.Diagnostics.CodeAnalysis;

namespace MyBots.Modules.Common.Handling;

public interface IModelRegistry
{
    void Register(string moduleName, Type modelType);

    bool TryGetDescription(string moduleName, string typeName, [NotNullWhen(true)] out ModelBindingDescription? description);
}
