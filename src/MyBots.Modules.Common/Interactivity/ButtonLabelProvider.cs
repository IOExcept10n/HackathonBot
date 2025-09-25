using System.Collections.ObjectModel;
using System.Reflection;

namespace MyBots.Modules.Common.Interactivity;

public class ButtonLabelProvider : IButtonLabelProvider
{
    private readonly IReadOnlyDictionary<string, ButtonLabel> _labels;

    public ButtonLabelProvider()
    {
        _labels = LoadLabelsFromAssembly(Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly());
    }

    public ButtonLabel GetLabel(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        return _labels.TryGetValue(key, out var label) ? label : "";
    }

    private static IReadOnlyDictionary<string, ButtonLabel> LoadLabelsFromAssembly(Assembly asm)
    {
        var dict = new Dictionary<string, ButtonLabel>(StringComparer.Ordinal);

        var typesWithAttr = asm.GetTypes()
            .Where(t => t.GetCustomAttribute<LabelsStorageAttribute>(false) != null);

        foreach (var type in typesWithAttr)
        {
            // Public static fields
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var f in fields)
            {
                try
                {
                    var val = f.GetValue(null);
                    if (val != null)
                    {
                        var key = f.Name;
                        if (!dict.ContainsKey(key))
                            dict[key] = (ButtonLabel)val;
                    }
                }
                catch
                {
                }
            }

            // Public static properties without index parameters and with getter
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.GetIndexParameters().Length == 0 && p.GetGetMethod() != null);

            foreach (var p in props)
            {
                try
                {
                    var val = p.GetValue(null);
                    if (val != null)
                    {
                        var key = p.Name;
                        if (!dict.ContainsKey(key))
                            dict[key] = (ButtonLabel)val;
                    }
                }
                catch
                {
                }
            }
        }

        return new ReadOnlyDictionary<string, ButtonLabel>(dict);
    }
}