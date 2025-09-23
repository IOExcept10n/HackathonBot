using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MyBots.Core.Models;

namespace MyBots.Core.Fsm;

/// <summary>
/// Default implementation of the state registry.
/// </summary>
public class StateRegistry(ILogger<StateRegistry> logger) : IStateRegistry
{
    private readonly ILogger<StateRegistry> _logger = logger;
    private readonly Dictionary<string, StateDefinition> _states = [];
    private readonly Dictionary<string, bool> _moduleStatus = [];

    public void Register(StateDefinition def)
    {
        if (_states.ContainsKey(def.StateId))
        {
            _logger.LogWarning("State {StateId} is already registered, overwriting", def.StateId);
        }

        _states[def.StateId] = def;
        
        if (!_moduleStatus.ContainsKey(def.Module))
        {
            _moduleStatus[def.Module] = true;
        }
    }

    public bool TryGet(string stateId, [NotNullWhen(true)] out StateDefinition? def)
    {
        if (_states.TryGetValue(stateId, out def))
        {
            return IsModuleEnabled(def.Module);
        }

        def = null;
        return false;
    }

    public IEnumerable<StateDefinition> ListForRole(Role role)
    {
        return _states.Values
            .Where(s => s.AllowedRoles.Contains(role) && IsModuleEnabled(s.Module));
    }

    public void EnableModule(string moduleId, bool enabled)
    {
        _moduleStatus[moduleId] = enabled;
        _logger.LogInformation("Module {ModuleId} is now {Status}", moduleId, enabled ? "enabled" : "disabled");
    }

    public bool IsModuleEnabled(string moduleId)
    {
        return _moduleStatus.TryGetValue(moduleId, out var enabled) && enabled;
    }
}