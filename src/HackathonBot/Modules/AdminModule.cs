using System.Security.Cryptography;
using System.Text;
using HackathonBot.Models;
using HackathonBot.Properties;
using HackathonBot.Repository;
using HackathonBot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBots.Core.Fsm.States;
using MyBots.Core.Localization;
using MyBots.Modules.Common;
using MyBots.Modules.Common.Interactivity;
using MyBots.Modules.Common.Roles;
using Telegram.Bot;

namespace HackathonBot.Modules;

internal class AdminModule(
    IStateRegistry states,
    ILocalizationService localization,
    ITelegramUserService userService,
    IBotUserRoleRepository roles,
    ITeamRepository teams,
    IParticipantRepository participants,
    ILogger<AdminModule> logger) :
    ModuleBase(Labels.Administration, [Roles.Admin], states, localization)
{
    private readonly IParticipantRepository _participants = participants;
    private readonly IBotUserRoleRepository _roles = roles;
    private readonly ITeamRepository _teams = teams;
    private readonly ITelegramUserService _users = userService;
    private readonly ILogger<AdminModule> _logger = logger;

    [MenuState(nameof(Localization.ConfirmUserDeletion), BackButton = false)]
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
    [MenuItem(nameof(Labels.UploadParticipantsList))]
    [MenuItem(nameof(Labels.ManageParticipants))]
    [MenuItem(nameof(Labels.ManageTeams))]
    [MenuItem(nameof(Labels.ManageOrganizers))]
    [MenuItem(nameof(Labels.DeleteParticipant))]
    public override async Task<StateResult> OnModuleRootAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.DeleteParticipant))
            return ToState(nameof(OnRequestUsernameForDeleteAsync));
        else if (ctx.Matches(Labels.RegisterParticipant))
            return ToState(nameof(OnRequestUsernameForCreationAsync), nameof(Roles.Participant));
        else if (ctx.Matches(Labels.RegisterAdmin))
            return ToState(nameof(OnSelectAdminRoleAsync), nameof(Roles.Participant));
        else if (ctx.Matches(Labels.ManageParticipants))
        {
            StringBuilder lst = new();
            lst.AppendLine(Localization.ParticipantsList);
            var participants = from participant in _participants.GetAll().Include(x => x.Team).AsNoTracking()
                               let teamName = participant.Team != null ? participant.Team.Name : "*"
                               orderby teamName
                               group participant by teamName;
            foreach (var group in participants)
            {
                lst.AppendLine(group.Key + ":");
                foreach (var participant in group)
                {
                    lst.AppendLine("- " + participant.FormatDisplay());
                }
                lst.AppendLine();
            }
            await ctx.ReplyAsync(lst.ToString());
            return ToState(nameof(OnManageParticipantsAsync));
        }
        else if (ctx.Matches(Labels.ManageTeams))
        {
            StringBuilder lst = new();
            lst.AppendLine(Localization.TeamsList);
            foreach (var team in _teams.GetAll().AsNoTracking().OrderBy(x => x.Name))
            {
                lst.AppendLine(team.Name);
            }
            await ctx.ReplyAsync(lst.ToString());
            return ToState(nameof(OnManageTeamsAsync));
        }
        else if (ctx.Matches(Labels.ManageOrganizers))
        {
            StringBuilder lst = new();
            lst.AppendLine(Localization.OrganizersList);
            var roles = _roles.GetAll().AsNoTracking()
                .Where(x => x.RoleId == RoleIndex.Organizer || x.RoleId == RoleIndex.Admin)
                .OrderBy(x => x.Username);
            foreach (var role in roles)
            {
                lst.AppendLine(role.FormatDisplay());
            }
            await ctx.ReplyAsync(lst.ToString());
            return ToState(nameof(OnManageOrganizersAsync));
        }
        else if (ctx.Matches(Labels.UploadParticipantsList))
            return ToState(nameof(OnUploadParticipantsListAsync));

        return InvalidInput(ctx);
    }

    [PromptState<Unit>(nameof(Localization.SendUsersFile), AllowFileInput = true, AllowTextInput = false)]
    public async Task<StateResult> OnUploadParticipantsListAsync(PromptStateContext<Unit> ctx)
    {
        if (ctx.Message is FileMessageContent file)
        {
            try
            {
                using MemoryStream stream = new();
                var info = await ctx.BotClient.GetInfoAndDownloadFile(file.FileId, stream, ctx.CancellationToken);
                stream.Position = 0;
                using StreamReader reader = new(stream);
                // Skip the first line.
                await reader.ReadLineAsync(ctx.CancellationToken);
                string? line;

                List<(string Name, string Tg, string Team)> participants = [];
                StringBuilder bd = new(), data = new();
                bd.AppendLine(Localization.CheckTeamUploadFileContents).AppendLine();

                while ((line = await reader.ReadLineAsync(ctx.CancellationToken)) != null)
                {
                    data.AppendLine(line);
                    var parts = line.Split(';');
                    string name = parts[0].Trim();
                    string tg = parts[1].Trim()
                     .Replace("https://t.me/", "")
                     .Replace("http://t.me/", "")
                     .Replace("t.me/", "")
                     .Replace("@", "");
                    string team = parts[2].Trim();
                    participants.Add((name, tg, team));
                }

                foreach (var g in participants.GroupBy(x => x.Team))
                {
                    bd.AppendLine(g.Key + ":");
                    foreach (var p in g) bd.AppendLine($"- {p.Name} (@{p.Tg})");
                    bd.AppendLine();
                }

                await ctx.ReplyAsync(bd.ToString());
                return ToState(nameof(OnConfirmUploadUsersAsync), data.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Incorrect file format, file cannot be read.");
                return InvalidInput(ctx);
            }
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.ConfirmUpload), BackButton = false)]
    [MenuItem(nameof(Labels.Yes))]
    [MenuItem(nameof(Labels.No))]
    public async Task<StateResult> OnConfirmUploadUsersAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.Yes))
        {
            string[] lines = ctx.StateData.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var line in lines)
            {
                var parts = line.Split(';');
                string name = parts[0].Trim();
                string tg = parts[1].Trim()
                    .Replace("https://t.me/", "")
                    .Replace("http://t.me/", "")
                    .Replace("t.me/", "")
                    .Replace("@", "");
                string team = parts[2].Trim();
                await _users.RegisterParticipantAsync(team, tg, name, ctx.CancellationToken);
            }
            return ToRoot(message: Localization.UploadCompleted);
        }

        return ToRoot();
    }

    [PromptState<string>(nameof(Localization.InputUserName))]
    public Task<StateResult> OnManageOrganizersAsync(PromptStateContext<string> ctx)
    {
        return Task.FromResult(Back(ctx, message: "Not implemented."));
    }

    [PromptState<string>(nameof(Localization.InputUserTeam))]
    public Task<StateResult> OnManageTeamsAsync(PromptStateContext<string> ctx)
    {
        return Task.FromResult(Back(ctx, message: "Not implemented."));
    }

    [PromptState<string>(nameof(Localization.InputUserName))]
    public Task<StateResult> OnManageParticipantsAsync(PromptStateContext<string> ctx)
    {
        return Task.FromResult(Back(ctx, message: "Not implemented."));
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
        await _users.RegisterParticipantAsync(teamName, nickname, name, ctx.CancellationToken);
        return ToRoot(message: Localization.UserAdded);
    }

    [MenuState(nameof(Localization.SelectRole), ParentStateName = RootStateName)]
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
}
