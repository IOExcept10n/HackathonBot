using MyBots.Core.Fsm.States;
using MyBots.Core.Localization;
using MyBots.Modules.Common;

namespace HackathonBot.Modules
{
    internal class HackathonModule(IStateRegistry stateRegistry, ILocalizationService localization) :
        ModuleBase(Labels.Hackathon, [Roles.User], stateRegistry, localization)
    {
        [MenuItem(nameof(Labels.Yes))]
        [MenuItem(nameof(Labels.No))]
        public override Task<StateResult> OnModuleRootAsync(ModuleStateContext ctx)
        {
            if (int.TryParse(ctx.StateData, out int v))
                v++;

            return Task.FromResult(Retry(ctx, v.ToString(), $"Hello, world! Iteration {v}"));
        }
    }
}
