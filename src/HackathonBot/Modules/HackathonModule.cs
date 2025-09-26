using HackathonBot.Models;
using HackathonBot.Properties;
using HackathonBot.Repository;
using MyBots.Core.Fsm.States;
using MyBots.Core.Localization;
using MyBots.Modules.Common;
using MyBots.Modules.Common.Interactivity;

namespace HackathonBot.Modules;

internal class HackathonModule(IParticipantRepository participants, IStateRegistry stateRegistry, ILocalizationService localization) :
    ModuleBase(Labels.Hackathon, [Roles.Participant], stateRegistry, localization)
{
    private readonly IParticipantRepository _participants = participants;

    [MenuItem(nameof(Labels.Case))]
    [MenuItem(nameof(Labels.MyTeam))]
    public override async Task<StateResult> OnModuleRootAsync(ModuleStateContext ctx)
    {
        var participant = await GetParticipantAsync(ctx);
        if (participant == null)
            return InvalidInput(ctx);

        if (ctx.Matches(Labels.Case))
        {
            return Retry(ctx, message: "Not implemented yet.");
        }
        else if (ctx.Matches(Labels.MyTeam))
        {
            if (participant.Team == null)
            {
                return Retry(ctx, message: Localization.NoTeamWarning);
            }
            string name = participant.FullName.Split(' ').ElementAtOrDefault(1) ?? participant.FullName;
            return Retry(ctx, message: string.Format(Localization.TeamInfo, name, participant.Team.Name, participant.Team.Case));
        }
        return InvalidInput(ctx);
    }

    private async Task<Participant?> GetParticipantAsync(ModuleStateContext ctx) => await _participants.FindByTelegramIdAsync(ctx.User.TelegramId);
}
