using System.Diagnostics.CodeAnalysis;

namespace MyBots.Modules.Common;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public abstract class FsmStateAttribute(string messageResourceKey) : Attribute
{
    public string? StateName { get; init; }

    public string? ParentStateName { get; init; } = "root";

    public string MessageResourceKey { get; } = messageResourceKey;
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class MenuStateAttribute(string messageResourceKey) : FsmStateAttribute(messageResourceKey)
{
    public bool BackButton { get; init; } = true;
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class MenuRowAttribute(params string[] labelKeys) : Attribute
{
    public string[] LabelKeys { get; init; } = labelKeys;
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class MenuItemAttribute(string labelKey) : MenuRowAttribute(labelKey)
{
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class InheritKeyboardAttribute : Attribute;
[AttributeUsage(AttributeTargets.Method)]
public sealed class RemoveKeyboardAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public abstract class PromptStateAttribute(Type inputType, string messageResourceKey) : FsmStateAttribute(messageResourceKey)
{
    public bool AllowFileInput { get; init; }

    public bool AllowTextInput { get; init; } = true;

    public Type InputType { get; } = inputType;

    public bool BackButton { get; init; } = true;
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class PromptStateAttribute<T>(string messageResourceKey) : PromptStateAttribute(typeof(T), messageResourceKey);

[AttributeUsage(AttributeTargets.Method)]
public abstract class ModelPromptStateAttribute(Type inputType) : FsmStateAttribute(string.Empty)
{
    public Type InputType { get; } = inputType;
    public bool BackButton { get; init; } = true;
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class ModelPromptStateAttribute<T>() : ModelPromptStateAttribute(typeof(T)) where T : new();

public readonly record struct Unit : IParsable<Unit>
{
    public static Unit Parse(string s, IFormatProvider? provider) => default;

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Unit result)
    {
        result = default;
        return false;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class LabelsStorageAttribute : Attribute;
