using HackathonBot.Models;
using HackathonBot.Properties;
using HackathonBot.Repository;
using HackathonBot.Services;
using MyBots.Core.Fsm.States;
using MyBots.Core.Localization;
using MyBots.Modules.Common;
using MyBots.Modules.Common.Interactivity;
using MyBots.Modules.Common.Roles;

namespace HackathonBot.Modules;

internal class AdminModule(
    IStateRegistry states,
    ILocalizationService localization,
    ITelegramUserService userService,
    IBotUserRoleRepository roles,
    ITeamRepository teams,
    IParticipantRepository participants) :
    ModuleBase(Labels.Administration, [Roles.Admin], states, localization)
{
    private readonly IParticipantRepository _participants = participants;
    private readonly IBotUserRoleRepository _roles = roles;
    private readonly ITeamRepository _teams = teams;
    private readonly ITelegramUserService _users = userService;

    [MenuState(nameof(Localization.ConfirmUserDeletion))]
    [MenuItem(nameof(Labels.Yes))]
    [MenuItem(nameof(Labels.No))]
    public async Task<StateResult> OnConfirmDeleteUserAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.Yes))
        {
            await _users.DeleteUserAsync(ctx.StateData, ctx.CancellationToken);
            return ToRoot(message: Localization.UserDeleted);
        }
        else if (ctx.Matches(Labels.No))
        {
            return ToState(RootStateId);
        }
        return InvalidInput(ctx);
    }

    [MenuItem(nameof(Labels.RegisterParticipant))]
    [MenuItem(nameof(Labels.RegisterAdmin))]
    [MenuItem(nameof(Labels.DeleteParticipant))]
    public override Task<StateResult> OnModuleRootAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.DeleteParticipant))
            return Task.FromResult(ToState(nameof(OnRequestUsernameForDeleteAsync)));
        else if (ctx.Matches(Labels.RegisterParticipant))
            return Task.FromResult(ToState(nameof(OnRequestUsernameForCreationAsync), nameof(Roles.Participant)));
        else if (ctx.Matches(Labels.RegisterAdmin))
            return Task.FromResult(ToState(nameof(OnSelectAdminRoleAsync), nameof(Roles.Participant)));
        return Task.FromResult(InvalidInput(ctx));
    }

    [PromptState<string>(nameof(Localization.InputUserFullName))]
    public Task<StateResult> OnRequestUserFullNameAsync(PromptStateContext<string> ctx)
        => Task.FromResult(ToState(nameof(OnRequestUserTeamAsync), $"{ctx.StateData}:{ctx.Input.GetValueOrDefault()}"));

    [PromptState<string>(nameof(Localization.InputUserName))]
    public async Task<StateResult> OnRequestUsernameForCreationAsync(PromptStateContext<string> ctx)
    {
        string name = ctx.Input.GetValueOrDefault();
        long? existenceCheck = await _users.GetTelegramIdByUsernameAsync(name, ctx.CancellationToken);
        if (existenceCheck != null)
            return ToRoot(message: Localization.UserAlreadyExists);

        return ctx.StateData switch
        {
            nameof(Roles.Participant) => ToState(nameof(OnRequestUserFullNameAsync), name),
            nameof(Roles.Organizer) => await CreateWithRole(name, Roles.Organizer, ctx.CancellationToken),
            nameof(Roles.Admin) => await CreateWithRole(name, Roles.Admin, ctx.CancellationToken),
            _ => Fail(),
        };
    }

    [PromptState<string>(nameof(Localization.InputUserName))]
    public async Task<StateResult> OnRequestUsernameForDeleteAsync(PromptStateContext<string> ctx)
    {
        string username = ctx.Input.GetValueOrDefault().ToLowerInvariant();
        var role = await _users.CheckUserByNameAsync(username, ctx.CancellationToken);
        switch (role)
        {
            case RoleIndex.Participant:
                await ctx.ReplyAsync($"{Localization.UserRoleInfo} {Localization.Participant}");
                return ToState(nameof(OnConfirmDeleteUserAsync), username);

            case RoleIndex.Organizer:
                await ctx.ReplyAsync($"{Localization.UserRoleInfo} {Localization.Organizer}");
                return ToState(nameof(OnConfirmDeleteUserAsync), username);

            case RoleIndex.Admin:
                await ctx.ReplyAsync($"{Localization.UserRoleInfo} {Localization.Admin}");
                return ToState(nameof(OnConfirmDeleteUserAsync), username);

            default:
                return ToRoot(message: Localization.UserNotFound);
        }
    }

    [PromptState<string>(nameof(Localization.InputUserTeam))]
    public async Task<StateResult> OnRequestUserTeamAsync(PromptStateContext<string> ctx)
    {
        string teamName = ctx.Input.GetValueOrDefault();
        var parts = ctx.StateData.Split(':');
        string nickname = parts[0];
        string name = parts[1];
        await RegisterUser(teamName, nickname, name, ctx.CancellationToken);
        return ToRoot(message: Localization.UserAdded);
    }

    [MenuState(nameof(Localization.SelectRole))]
    [MenuItem(nameof(Labels.Admin))]
    [MenuItem(nameof(Labels.Organizer))]
    public Task<StateResult> OnSelectAdminRoleAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.Admin))
            return Task.FromResult(ToState(nameof(OnRequestUsernameForCreationAsync), nameof(Roles.Admin)));
        if (ctx.Matches(Labels.Organizer))
            return Task.FromResult(ToState(nameof(OnRequestUsernameForCreationAsync), nameof(Roles.Organizer)));
        return Task.FromResult(InvalidInput(ctx));
    }

    private async Task<StateResult> CreateWithRole(string username, Role role, CancellationToken cancellationToken)
    {
        var userRole = new BotUserRole()
        {
            Username = username,
            Role = role
        };
        await _roles.AddAsync(userRole, cancellationToken);
        await _roles.SaveChangesAsync(cancellationToken);
        return ToRoot(message: Localization.UserAdded);
    }

    private async Task RegisterUser(string teamName, string nickname, string name, CancellationToken cancellationToken)
    {
        var team = await _teams.FindByNameAsync(teamName, cancellationToken);
        bool isLeader = false;
        if (team == null)
        {
            isLeader = true;
            team = new()
            {
                Name = teamName,
            };
            await _teams.AddAsync(team, cancellationToken);
            await _teams.SaveChangesAsync(cancellationToken);
        }
        var participant = new Participant()
        {
            IsLeader = isLeader,
            FullName = name,
            Nickname = nickname,
            TeamId = team.Id
        };
        await _participants.AddAsync(participant, cancellationToken);
        await _participants.SaveChangesAsync(cancellationToken);
    }
}
