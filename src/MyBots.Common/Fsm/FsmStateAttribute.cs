using MyBots.Core.Models;

namespace MyBots.Core.Fsm;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class FsmStateAttribute(string stateId) : Attribute
{
    public string StateId { get; } = stateId;
    public string Module { get; init; } = "default";
    public Role[] AllowedRoles { get; init; } = [];
    public bool IsRootForRole { get; init; } = false;
    public bool IsPromptState { get; init; } = false;
}