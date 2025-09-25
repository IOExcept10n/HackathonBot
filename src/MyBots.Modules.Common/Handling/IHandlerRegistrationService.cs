namespace MyBots.Modules.Common.Handling;

public interface IHandlerRegistrationService
{
    void RegisterModule(ModuleBase module, IStateHandlerRegistry registry);
}
