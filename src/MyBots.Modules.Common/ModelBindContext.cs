using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBots.Core.Fsm.States;
using MyBots.Modules.Common.Roles;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = MyBots.Core.Persistence.DTO.User;

namespace MyBots.Modules.Common;

public record ModelBindContext(
    User User,
    ChatId Chat,
    Role Role,
    MessageContent Message,
    ModelBindingDescription Binding,
    string StateData,
    ITelegramBotClient BotClient,
    CancellationToken CancellationToken)
    : ModuleStateContext(User, Chat, Role, Message, StateData, BotClient, CancellationToken)
{
}

public record ModelPromptContext<TModel>(
    User User,
    ChatId Chat,
    Role Role,
    MessageContent Message,
    TModel Model,
    string StateData,
    ITelegramBotClient BotClient,
    CancellationToken CancellationToken)
    : ModuleStateContext(User, Chat, Role, Message, StateData, BotClient, CancellationToken)
{
}

public record ModelProperty(string Name, string DisplayName, PropertyInfo Property);

public record ModelBindingDescription(string ModuleName, Type RequestedModelType, ModelProperty[] ModelProperties)
{
    public ModelProperty? NextProperty(ModelProperty current)
    {
        int index = Array.IndexOf(ModelProperties, current);
        if (index == ModelProperties.Length - 1)
            return null;
        if (index < 0)
            return ModelProperties[0];
        return ModelProperties[index + 1];
    }

    public ModelProperty? PropertyByName(string? propertyName) => Array.Find(ModelProperties, x => x.Name == propertyName);
}

public record ModelBindingBuilder(ModelBindingDescription Description, ModelProperty InputProperty, ValidationContext Validation, object Model)
{
    public static ModelBindingBuilder FromData(ModelBindingDescription binding, ModelBuilderData data, IServiceProvider services)
    {
        var model = data.Model.Deserialize(binding.RequestedModelType) ?? throw new InvalidOperationException();
        var property = binding.PropertyByName(data.PropertyName) ?? throw new InvalidOperationException();
        return new(
            binding,
            property,
            new(model, services, null)
            {
                MemberName = property.Name,
                DisplayName = property.DisplayName,
            },
            model);
    }

    public BindingResult AppendValue(object? propertyValue)
    {
        try
        {
            List<ValidationResult> validationResults = [];
            if (!Validator.TryValidateProperty(propertyValue, Validation, validationResults))
            {
                return new(true, validationResults);
            }
            InputProperty.Property.SetValue(Model, propertyValue);
            JsonElement element = ToJsonElement(Model);
            var nextProperty = Description.NextProperty(InputProperty);
            ModelBuilderData data = new(Description.RequestedModelType.FullName!, nextProperty?.Name, ToJsonElement(Model), nextProperty == null);
            return new(false, validationResults, data);
        }
        catch (Exception ex)
        {
            return new(true, [new(ex.Message)]);
        }
    }

    public T Build<T>()
    {
        if (!typeof(T).IsAssignableTo(Description.RequestedModelType))
            throw new ArgumentException($"Model cannot be converted to requested type. Expected type: {Description.RequestedModelType}.", typeof(T).Name);
        return (T)Model;
    }

    public readonly record struct BindingResult(bool HasError, ICollection<ValidationResult> ValidationErrors, ModelBuilderData Data = default);

    public static JsonElement ToJsonElement(object? obj)
    {
        if (obj is null)
        {
            using var nullDoc = JsonDocument.Parse("null");
            return nullDoc.RootElement.Clone();
        }
        
        var bytes = JsonSerializer.SerializeToUtf8Bytes(obj);
        using var doc = JsonDocument.Parse(bytes);
        return doc.RootElement.Clone();
    }
}

public readonly record struct ModelBuilderData(string TypeName, string? PropertyName, JsonElement Model, bool IsCompleted);
