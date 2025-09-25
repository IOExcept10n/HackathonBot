namespace MyBots.Modules.Common.Roles;

public record Role(string Name)
{
    public static UnknownRole Unknown { get; } = new();
}

public record UnknownRole() : Role(string.Empty);
