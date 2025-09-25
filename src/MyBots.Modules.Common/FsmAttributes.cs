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
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class MenuItemAttribute(string labelKey) : Attribute
{
    public string LabelKey { get; } = labelKey;
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public abstract class PromptStateAttribute(Type inputType, string messageResourceKey) : FsmStateAttribute(messageResourceKey)
{
    public bool AllowFileInput { get; init; }

    public bool AllowTextInput { get; init; } = true;

    public Type InputType { get; } = inputType;
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class PromptStateAttribute<T>(string messageResourceKey) : PromptStateAttribute(typeof(T), messageResourceKey);

[AttributeUsage(AttributeTargets.Class)]
public sealed class LabelsStorageAttribute : Attribute;
