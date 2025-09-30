using System.Text;
using HackathonBot.Models;
using HackathonBot.Properties;
using HackathonBot.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBots.Core.Fsm.States;
using MyBots.Core.Localization;
using MyBots.Modules.Common;
using MyBots.Modules.Common.Interactivity;
using Quartz.Util;
using Telegram.Bot;

namespace HackathonBot.Modules;

public class NotificationModule(
    ITeamRepository teams,
    IBotUserRoleRepository users,
    IParticipantRepository participants,
    ILogger<NotificationModule> logger,
    IStateRegistry states,
    ILocalizationService localization) :
    ModuleBase(Labels.Notifications, [Roles.Admin, Roles.Organizer], states, localization)
{
    private readonly ITeamRepository _teams = teams;
    private readonly IBotUserRoleRepository _users = users;
    private readonly IParticipantRepository _participants = participants;
    private readonly ILogger<NotificationModule> _logger = logger;

    private const string All = "all";
    private const string Participants = "participants";
    private const string Organizers = "organizers";

    [MenuItem(nameof(Labels.MailingToAll))]
    [MenuItem(nameof(Labels.MailingToParticipants))]
    [MenuItem(nameof(Labels.MailingToOrganizers))]
    [MenuItem(nameof(Labels.MailingToTeam))]
    [MenuItem(nameof(Labels.MailingPersonal))]
    public override async Task<StateResult> OnModuleRootAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.MailingToAll))
            return ToState(nameof(OnInputMessageAsync), All);
        else if (ctx.Matches(Labels.MailingToParticipants))
            return ToState(nameof(OnInputMessageAsync), Participants);
        else if (ctx.Matches(Labels.MailingToOrganizers))
            return ToState(nameof(OnInputMessageAsync), Organizers);
        else if (ctx.Matches(Labels.MailingToTeam))
        {
            StringBuilder teams = new();
            teams.AppendLine(Localization.TeamsList);
            foreach (var team in _teams.GetAll().AsNoTracking())
            {
                teams.AppendLine(team.Name);
            }
            await ctx.ReplyAsync(teams.ToString());
            return ToState(nameof(OnInputTeamAsync));
        }
        else if (ctx.Matches(Labels.MailingPersonal))
            return ToState(nameof(OnInputUserAsync));
        return InvalidInput(ctx);
    }

    [PromptState<string>(nameof(Localization.MessageRequestPrompt))]
    public Task<StateResult> OnInputMessageAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.Input.TryGetValue(out string message))
            return Task.FromResult(InvalidInput(ctx));
        return Task.FromResult(ToState(nameof(OnShowSenderNameAsync), ctx.StateData + Environment.NewLine + message));
    }

    [MenuState(nameof(Localization.ShowSenderNameRequest))]
    [MenuItem(nameof(Labels.Yes))]
    [MenuItem(nameof(Labels.No))]
    public async Task<StateResult> OnShowSenderNameAsync(ModuleStateContext ctx)
    {
        bool show = false;
        if (ctx.Matches(Labels.Yes)) show = true;
        else if (ctx.Matches(Labels.No)) show = false;
        else return InvalidInput(ctx);

        int firstLine = ctx.StateData.IndexOf(Environment.NewLine);

        string to = ctx.StateData[..firstLine];
        string message = ctx.StateData[(firstLine + Environment.NewLine.Length)..];

        int targets = 0;
        int actualTargets = 0;

        switch (to)
        {
            case All:
                {
                    targets = await _users.GetAll().AsNoTracking().CountAsync(x => x.RoleId != RoleIndex.Participant) +
                              await _participants.GetAll().AsNoTracking().CountAsync();
                    actualTargets = await _users.GetAll().AsNoTracking().Where(x => x.TelegramId != null).CountAsync(x => x.RoleId != RoleIndex.Participant) +
                                    await _participants.GetAll().AsNoTracking().Where(x => x.TelegramId != null).CountAsync();
                    break;
                }
            case Participants:
                {
                    targets = await _participants.GetAll().AsNoTracking().CountAsync();
                    actualTargets = await _participants.GetAll().AsNoTracking().Where(x => x.TelegramId != null).CountAsync();
                    break;
                }
            case Organizers:
                {
                    targets = await _users.GetAll().AsNoTracking().CountAsync(x => x.RoleId != RoleIndex.Participant);
                    actualTargets = await _users.GetAll().AsNoTracking().Where(x => x.TelegramId != null).CountAsync(x => x.RoleId != RoleIndex.Participant);
                    break;
                }
            default:
                {
                    if (to.StartsWith("team:"))
                    {
                        var team = await _teams.FindByNameAsync(to[5..]); // Skip 'team:' from tag
                        if (team == null)
                        {
                            targets = 0;
                            actualTargets = 0;
                        }
                        else
                        {
                            team = await _teams.GetWithMembersAsync(team.Id);
                            targets = team!.Members.Count;
                            actualTargets = team.Members.Count(x => x.IsLoggedIntoBot);
                        }
                    }
                    else if (to.StartsWith("user:"))
                    {
                        var user = await _users.FindByUsernameAsync(to[5..]); // Skip 'user:' from tag
                        if (user == null)
                        {
                            targets = 0;
                            actualTargets = 0;
                        }
                        else
                        {
                            targets = 1;
                            actualTargets = user.TelegramId != null ? 1 : 0;
                        }
                    }
                    else return Fail();
                    break;
                }
        }

        await ctx.ReplyAsync(Localization.MailingDetails.FormatInvariant(
            ctx.User.Name, // From: {0}
            show.AsEmoji().ToUnicode(), // Show author: {1}
            to, // To: {2}
            message, // Message: {3}
            targets, // Targets: {4}
            actualTargets)); // Actual targets: {5}

        return ToState(nameof(OnConfirmMailing), ctx.StateData + Environment.NewLine + show);
    }

    [MenuState(nameof(Localization.ConfirmMailing))]
    [MenuItem(nameof(Labels.Yes))]
    [MenuItem(nameof(Labels.No))]
    public async Task<StateResult> OnConfirmMailing(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.Yes))
        {
            var (Sent, Total) = await Mail(ctx);
            return Completed(Localization.MailingCompleted.FormatInvariant(Sent, Total));
        }
        else if (ctx.Matches(Labels.No))
        {
            return Completed(Localization.MailingRejected);
        }
        return InvalidInput(ctx);
    }

    private async Task<(int Sent, int Total)> Mail(ModuleStateContext ctx)
    {
        int sent = 0;
        int total = 0;

        IEnumerable<long?> targets = [];

        int firstLine = ctx.StateData.IndexOf(Environment.NewLine);
        int lastLine = ctx.StateData.LastIndexOf(Environment.NewLine);

        string to = ctx.StateData[..firstLine];
        string message = ctx.StateData[(firstLine + Environment.NewLine.Length)..lastLine];
        if (!bool.TryParse(ctx.StateData[(lastLine + Environment.NewLine.Length)..].Trim(), out bool show)) show = false;

        switch (to)
        {
            case All:
                targets = [.. from user in _users.GetAll().AsNoTracking() where user.RoleId != RoleIndex.Participant select user.TelegramId, 
                           .. from participant in _participants.GetAll().AsNoTracking() select participant.TelegramId];
                break;
            case Participants:
                targets = from participant in _participants.GetAll().AsNoTracking() select participant.TelegramId; break;
            case Organizers:
                targets = from user in _users.GetAll().AsNoTracking() where user.RoleId != RoleIndex.Participant select user.TelegramId; break;
            case { } when to.StartsWith("team:"):
                var team = await _teams.FindByNameAsync(to[5..]); // Skip 'team:' from tag
                if (team == null)
                {
                    targets = [];
                }
                else
                {
                    team = await _teams.GetWithMembersAsync(team.Id);
                    targets = from member in team!.Members select member.TelegramId;
                }
                break;
            case { } when to.StartsWith("user:"):
                var target = await _users.FindByUsernameAsync(to[5..]); // Skip 'user:' from tag
                if (target == null)
                {
                    targets = [];
                }
                else
                {
                    targets = [target.TelegramId];
                }
                break;
            default:
                return (0, 0);
        }

        if (show)
        {
            message = message + Environment.NewLine + Environment.NewLine + $"> @{ctx.User.Name}";
        }

        foreach (var tgId in targets)
        {
            total++;
            if (tgId is not long id)
                continue;
            try
            {
                await ctx.BotClient.SendMessage(id, message);
                sent++;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Couldn't send mailing message to {Id} because of an error.", id);
            }
        }

        return (sent, total);
    }

    [PromptState<string>(nameof(Localization.InputUserTeam))]
    public async Task<StateResult> OnInputTeamAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.Input.TryGetValue(out var teamName))
            return InvalidInput(ctx);
        var containsTeam = await _teams.GetAll().AsNoTracking().AnyAsync(x => x.Name == teamName);
        if (!containsTeam)
            return InvalidInput(ctx);
        return ToState(nameof(OnInputMessageAsync), $"team:{teamName}");
    }

    [PromptState<string>(nameof(Localization.InputUserName))]
    public async Task<StateResult> OnInputUserAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.Input.TryGetValue(out var userName))
            return InvalidInput(ctx);

        userName = userName.ToLower().Replace("@", "");

        var containsTeam = await _users.GetAll().AsNoTracking().AnyAsync(x => x.Username == userName);
        if (!containsTeam)
            return InvalidInput(ctx);
        return ToState(nameof(OnInputMessageAsync), $"user:{userName}");
    }
}