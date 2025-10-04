using System.Text;
using HackathonBot.Models;
using HackathonBot.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyBots.Core.Fsm.States;
using MyBots.Modules.Common;
using MyBots.Modules.Common.Interactivity;
using MyBots.Modules.Common.Roles;
using Quartz.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HackathonBot.Modules;

internal class AdminModule(IServiceProvider services) : BotModule(Labels.Administration, [Roles.Admin], services)
{
    private readonly ILogger<AdminModule> _logger = services.GetRequiredService<ILogger<AdminModule>>();

    [MenuState(nameof(Localization.ConfirmUserDeletion), BackButton = false)]
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public async Task<StateResult> OnConfirmDeleteUserAsync(ModuleStateContext ctx)
    {
        if (!ctx.TryGetData(out string? username))
            return Fail();

        if (ctx.Matches(Labels.Yes))
        {
            await Users.DeleteUserAsync(username, ctx.CancellationToken);
            return ToRootWithMessage(Localization.UserDeleted);
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
            return ToStateWith(nameof(OnRequestUsernameForCreationAsync), nameof(Roles.Participant));
        else if (ctx.Matches(Labels.RegisterAdmin))
            return ToStateWith(nameof(OnSelectAdminRoleAsync), nameof(Roles.Participant));
        else if (ctx.Matches(Labels.ManageParticipants))
        {
            StringBuilder lst = new();
            lst.AppendLine(Localization.ParticipantsList);
            var participants = from participant in Participants.GetAll().Include(x => x.Team).AsNoTracking()
                               let teamName = participant.Team != null ? participant.Team.Name : "*"
                               orderby teamName
                               group participant by teamName;
            foreach (var group in participants)
            {
                lst.AppendLine(group.Key + ":");
                foreach (var participant in group)
                {
                    lst.Append("- ").Append(participant.FormatDisplay()).Append(' ').AppendLine(participant.IsLoggedIntoBot.AsEmoji().ToUnicode());
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

            List<List<KeyboardButton>> buttonLayout = [];

            foreach (var team in Teams.GetAll().AsNoTracking().OrderBy(x => x.Name))
            {
                lst.Append("- ").Append(" [").Append(team.Case).Append("]: ").AppendLine(team.Name);
                buttonLayout.Add([new(team.Name)]);
            }

            ReplyKeyboardMarkup markup = new(buttonLayout);

            await ctx.BotClient.SendMessage(ctx.Chat, lst.ToString(), replyMarkup: markup);

            return ToState(nameof(OnManageTeamsAsync));
        }
        else if (ctx.Matches(Labels.ManageOrganizers))
        {
            StringBuilder lst = new();
            lst.AppendLine(Localization.OrganizersList);
            var roles = BotRoles.GetAll().AsNoTracking()
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
                return ToStateWith(nameof(OnConfirmUploadUsersAsync), participants);
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
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public async Task<StateResult> OnConfirmUploadUsersAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.Yes))
        {
            if (!ctx.TryGetData(out List<(string Name, string Tg, string Team)>? participants))
                return Fail();

            foreach (var (name, tg, team) in participants)
            {
                await Users.RegisterParticipantAsync(team, tg, name, ctx.CancellationToken);
            }
            return ToRootWithMessage(Localization.UploadCompleted);
        }

        return ToRoot();
    }

    [PromptState<string>(nameof(Localization.InputUserName))]
    public Task<StateResult> OnManageOrganizersAsync(PromptStateContext<string> ctx)
    {
        return Task.FromResult(Back(ctx, message: "Not implemented."));
    }

    [MenuState(nameof(Localization.InputUserTeam))]
    [InheritKeyboard]
    public async Task<StateResult> OnManageTeamsAsync(ModuleStateContext ctx)
    {
        if (ctx.Message is not TextMessageContent { Text: string teamName })
            return InvalidInput(ctx);
        var team = await Teams.FindByNameAsync(teamName, ctx.CancellationToken);
        if (team == null)
            return FailWithMessage(Localization.TeamNotFound);
        team = (await Teams.GetWithMembersAsync(team.Id, ctx.CancellationToken))!;

        StringBuilder members = new();
        foreach (var member in team.Members)
        {
            members.AppendLine("- " + member.FormatDisplay());
        }

        bool hasPresentation = !string.IsNullOrEmpty(team.Submission?.PresentationLink) ||
                               !string.IsNullOrEmpty(team.Submission?.PresentationFileUrl);
        bool hasRepo = !string.IsNullOrEmpty(team.Submission?.RepoUrl);

        await ctx.ReplyAsync(Localization.TeamDetails.FormatInvariant(
            teamName,
            members,
            LocalizationService.GetString(team.Case.ToString()),
            hasPresentation.AsEmoji().ToUnicode(),
            hasRepo.AsEmoji().ToUnicode()));
        return ToStateWith(nameof(OnTeamMenuAsync), teamName);
    }

    [MenuState(nameof(Localization.SelectTeamAction))]
    [MenuItem(nameof(Labels.GetTeamSubmission))]
    [MenuItem(nameof(Labels.RenameTeam))]
    [MenuItem(nameof(Labels.DeleteTeam))]
    public async Task<StateResult> OnTeamMenuAsync(ModuleStateContext ctx)
    {
        if (!ctx.TryGetData(out string? teamName))
            return Fail();

        if (ctx.Matches(Labels.GetTeamSubmission))
        {
            var team = await Teams.FindByNameAsync(teamName, ctx.CancellationToken);
            if (team == null)
                return FailWithMessage(message: Localization.TeamNotFound);
            var submission = team.Submission;

            if (submission == null)
            {
                await ctx.ReplyAsync(Localization.NoPresentationYet);
                await ctx.ReplyAsync(Localization.NoRepoYet);
                return ToRoot();
            }

            if (!string.IsNullOrEmpty(submission.PresentationFileUrl))
            {
                InputFile file = InputFile.FromFileId(submission.PresentationFileUrl);
                await ctx.BotClient.SendDocument(ctx.Chat, file, Localization.PresentationFileInfo.FormatInvariant(team.Name));
            }
            else if (!string.IsNullOrEmpty(submission.PresentationLink))
            {
                await ctx.ReplyAsync(Localization.PresentationLinkInfo.FormatInvariant(team.Name, submission.PresentationLink));
            }
            else
            {
                await ctx.ReplyAsync(Localization.NoPresentationYet);
            }

            if (!string.IsNullOrEmpty(submission.RepoUrl))
            {
                await ctx.ReplyAsync(Localization.RepoLinkInfo.FormatInvariant(team.Name, submission.RepoUrl));
            }
            else
            {
                await ctx.ReplyAsync(Localization.NoRepoYet);
            }

            return ToRoot();
        }
        if (ctx.Matches(Labels.SetTeamCase))
        {
            return ToStateWith(nameof(OnSetTeamCaseAsync), teamName);
        }
        if (ctx.Matches(Labels.RenameTeam))
        {
            return ToStateWith(nameof(OnInputNewTeamNameAsync), teamName);
        }
        if (ctx.Matches(Labels.DeleteTeam))
        {
            return ToStateWith(nameof(OnConfirmDeleteTeamAsync), teamName);
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.SelectCase))]
    [MenuItem(nameof(Labels.CaseLD))]
    [MenuItem(nameof(Labels.CaseTBank))]
    public async Task<StateResult> OnSetTeamCaseAsync(ModuleStateContext ctx)
    {
        if (!ctx.TryGetData(out string? teamName))
            return Fail();

        var team = await Teams.FindByNameAsync(teamName, ctx.CancellationToken);
        if (team == null)
            return Fail();

        if (ctx.Matches(Labels.CaseLD))
        {
            team.Case = Case.LD;
        }
        else if (ctx.Matches(Labels.CaseTBank))
        {
            team.Case = Case.TBank;
        }
        await Teams.SaveChangesAsync(ctx.CancellationToken);
        return Completed(Localization.TeamCaseSet);
    }

    [MenuState(nameof(Localization.ConfirmTeamDeletion), BackButton = false)]
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public async Task<StateResult> OnConfirmDeleteTeamAsync(ModuleStateContext ctx)
    {
        if (!ctx.TryGetData(out string? teamName))
            return Fail();

        if (ctx.Matches(Labels.Yes))
        {
            var team = await Teams.FindByNameAsync(teamName, ctx.CancellationToken);
            if (team == null)
                return FailWithMessage(message: Localization.TeamNotFound);
            await Teams.DeleteAsync(team, ctx.CancellationToken);
            await Teams.SaveChangesAsync(ctx.CancellationToken);
            return Completed(Localization.TeamDeleted);
        }
        else if (ctx.Matches(Labels.No))
        {
            return Completed(Localization.Cancel);
        }
        return InvalidInput(ctx);
    }

    [PromptState<string>(nameof(Localization.InputUserTeam))]
    public async Task<StateResult> OnInputNewTeamNameAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.TryGetData(out string? oldName))
            return Fail();

        if (!ctx.Input.TryGetValue(out string name))
            return InvalidInput(ctx);
        var team = await Teams.FindByNameAsync(oldName);
        if (team == null)
            return FailWithMessage(message: Localization.TeamNotFound);
        team.Name = name;
        await Teams.SaveChangesAsync(ctx.CancellationToken);
        return Completed(Localization.TeamRenamed);
    }

    [PromptState<string>(nameof(Localization.InputUserName))]
    public async Task<StateResult> OnManageParticipantsAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.Input.TryGetValue(out string name))
            return InvalidInput(ctx);
        name = name.AsCanonicalNickname();
        var participant = await Participants.FindByUsernameAsync(name, ctx.CancellationToken);
        if (participant == null)
            return FailWithMessage(Localization.UserNotFound);

        await ctx.ReplyAsync(Localization.ParticipantDetails.FormatInvariant(
            participant.FullName,
            participant.Nickname,
            participant.IsLoggedIntoBot.AsEmoji().ToUnicode(),
            participant.Team?.Name));
        return ToStateWith(nameof(OnParticipantMenuAsync), name);
    }

    [MenuState(nameof(Localization.SelectUserAction))]
    [MenuItem(nameof(Labels.ChangeParticipantTeam))]
    [MenuItem(nameof(Labels.DeleteParticipant))]
    public async Task<StateResult> OnParticipantMenuAsync(ModuleStateContext ctx)
    {
        if (!ctx.TryGetData(out string? username))
            return Fail();

        var participant = await Participants.FindByUsernameAsync(username, ctx.CancellationToken);
        if (participant == null)
            return FailWithMessage(message: Localization.UserNotFound);

        if (ctx.Matches(Labels.ChangeParticipantTeam))
        {
            var teams = Teams.GetAll().AsNoTracking().Include(t => t.Members).Where(x => x.Members.Count < 5);
            StringBuilder availableTeams = new();
            availableTeams.AppendLine(Localization.TeamsList);
            List<List<KeyboardButton>> buttonLayout = [];
            foreach (var team in teams)
            {
                availableTeams.Append("- ").AppendLine(team.Name);
                buttonLayout.Add([new(team.Name)]);
            }

            ReplyKeyboardMarkup markup = new(buttonLayout);

            await ctx.BotClient.SendMessage(ctx.Chat, availableTeams.ToString(), replyMarkup: markup);
            return ToStateWith(nameof(OnSelectParticipantTeamAsync), username);
        }
        else if (ctx.Matches(Labels.DeleteParticipant))
        {
            return ToStateWith(nameof(OnConfirmDeleteUserAsync), username);
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.InputUserTeam))]
    [InheritKeyboard]
    public async Task<StateResult> OnSelectParticipantTeamAsync(ModuleStateContext ctx)
    {
        if (ctx.Message is not TextMessageContent text)
            return InvalidInput(ctx);
        if (!ctx.TryGetData(out string? username))
            return Fail();

        var participant = await Participants.FindByUsernameAsync(username, ctx.CancellationToken);
        if (participant == null)
            return FailWithMessage(message: Localization.UserNotFound);

        var team = await Teams.FindByNameAsync(text.Text, ctx.CancellationToken);
        if (team == null)
        {
            team = new()
            {
                Name = text.Text,
            };
            await Teams.AddAsync(team, ctx.CancellationToken);
            await Teams.SaveChangesAsync();
        }

        participant.TeamId = team.Id;
        await Participants.SaveChangesAsync();
        return Completed(Localization.ParticipantTeamChanged);
    }

    [PromptState<string>(nameof(Localization.InputUserFullName))]
    public Task<StateResult> OnRequestUserFullNameAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.TryGetData(out ParticipantInputModel participant))
            return Task.FromResult(Fail());
        return Task.FromResult(ToStateWith(nameof(OnRequestUserTeamAsync), participant with { FullName = ctx.Input.GetValueOrDefault()}));
    }

    private readonly record struct ParticipantInputModel(string Username, string FullName);

    [PromptState<string>(nameof(Localization.InputUserName))]
    public async Task<StateResult> OnRequestUsernameForCreationAsync(PromptStateContext<string> ctx)
    {
        string name = ctx.Input.GetValueOrDefault();
        long? existenceCheck = await Users.GetTelegramIdByUsernameAsync(name, ctx.CancellationToken);
        if (existenceCheck != null)
            return ToRootWithMessage(message: Localization.UserAlreadyExists);
        if (!ctx.TryGetData(out string? data))
            return Fail();

        return data switch
        {
            nameof(Roles.Participant) => ToStateWith(nameof(OnRequestUserFullNameAsync), new ParticipantInputModel(name, string.Empty)),
            nameof(Roles.Organizer) => await CreateWithRole(name, Roles.Organizer, ctx.CancellationToken),
            nameof(Roles.Admin) => await CreateWithRole(name, Roles.Admin, ctx.CancellationToken),
            _ => Fail(),
        };
    }

    [PromptState<string>(nameof(Localization.InputUserName))]
    public async Task<StateResult> OnRequestUsernameForDeleteAsync(PromptStateContext<string> ctx)
    {
        string username = ctx.Input.GetValueOrDefault().ToLowerInvariant();
        var role = await Users.CheckUserByNameAsync(username, ctx.CancellationToken);
        switch (role)
        {
            case RoleIndex.Participant:
                await ctx.ReplyAsync($"{Localization.UserRoleInfo} {Localization.Participant}");
                return ToStateWith(nameof(OnConfirmDeleteUserAsync), username);

            case RoleIndex.Organizer:
                await ctx.ReplyAsync($"{Localization.UserRoleInfo} {Localization.Organizer}");
                return ToStateWith(nameof(OnConfirmDeleteUserAsync), username);

            case RoleIndex.Admin:
                await ctx.ReplyAsync($"{Localization.UserRoleInfo} {Localization.Admin}");
                return ToStateWith(nameof(OnConfirmDeleteUserAsync), username);

            default:
                return ToRootWithMessage(Localization.UserNotFound);
        }
    }

    [PromptState<string>(nameof(Localization.InputUserTeam))]
    public async Task<StateResult> OnRequestUserTeamAsync(PromptStateContext<string> ctx)
    {
        string teamName = ctx.Input.GetValueOrDefault();
        if (!ctx.TryGetData(out ParticipantInputModel participant))
            return Fail();
        await Users.RegisterParticipantAsync(teamName, participant.Username, participant.FullName, ctx.CancellationToken);
        return ToRootWithMessage(Localization.UserAdded);
    }

    [MenuState(nameof(Localization.SelectRole), ParentStateName = RootStateName)]
    [MenuRow(nameof(Labels.Admin), nameof(Labels.Organizer))]
    public Task<StateResult> OnSelectAdminRoleAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.Admin))
            return Task.FromResult(ToStateWith(nameof(OnRequestUsernameForCreationAsync), nameof(Roles.Admin)));
        if (ctx.Matches(Labels.Organizer))
            return Task.FromResult(ToStateWith(nameof(OnRequestUsernameForCreationAsync), nameof(Roles.Organizer)));
        return Task.FromResult(InvalidInput(ctx));
    }

    private async Task<StateResult> CreateWithRole(string username, Role role, CancellationToken cancellationToken)
    {
        var userRole = new BotUserRole()
        {
            Username = username,
            Role = role
        };
        await BotRoles.AddAsync(userRole, cancellationToken);
        await BotRoles.SaveChangesAsync(cancellationToken);
        return ToRootWithMessage(Localization.UserAdded);
    }
}
