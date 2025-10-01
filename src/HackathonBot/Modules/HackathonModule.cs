using System.Text;
using HackathonBot.Models;
using HackathonBot.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyBots.Core.Fsm.States;
using MyBots.Modules.Common;
using MyBots.Modules.Common.Interactivity;
using Quartz.Util;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HackathonBot.Modules;

internal class HackathonModule(IServiceProvider services) : BotModule(Labels.Hackathon, [Roles.Participant], services)
{
    private readonly HackathonConfig _config = services.GetRequiredService<IOptions<HackathonConfig>>().Value;

    [MenuItem(nameof(Labels.Case))]
    [MenuItem(nameof(Labels.MyTeam))]
    public override async Task<StateResult> OnModuleRootAsync(ModuleStateContext ctx)
    {
        var participant = await GetParticipantAsync(ctx);
        if (participant == null)
            return Fail();

        if (ctx.Matches(Labels.Case))
        {
            // If hackathon has been started.
            if (DateTime.UtcNow > _config.HackathonStart)
            {
                if (participant.Team == null)
                    return Fail(message: Localization.NoTeamWarning);

                bool presentationUploaded = false, repoUploaded = false;
                string lastUpd = "-";

                var submission = participant.Team.Submission;
                if (submission != null)
                {
                    presentationUploaded = !string.IsNullOrEmpty(submission.PresentationFileUrl) || !string.IsNullOrEmpty(submission.PresentationLink);
                    repoUploaded = !string.IsNullOrEmpty(submission.RepoUrl);
                    lastUpd = $"{submission.SubmittedAt} ({submission.SubmittedBy.FullName})";
                }

                var delta = _config.StopCode - DateTime.UtcNow;
                if (delta < TimeSpan.Zero)
                    delta = TimeSpan.Zero;

                await ctx.ReplyAsync(string.Format(Localization.UploadStatus,
                    participant.GetNameOnly(),
                    participant.Team.Name,
                    presentationUploaded.AsEmoji().ToUnicode(),
                    repoUploaded.AsEmoji().ToUnicode(),
                    lastUpd,
                    $"{(int)delta.TotalHours:00}:{delta:mm\\:ss}"));
                return ToState(nameof(OnCaseAsync_HackathonStarted));
            }
            else
            {
                return ToState(nameof(OnCaseAsync_HackathonNotStarted));
            }
        }
        else if (ctx.Matches(Labels.MyTeam))
        {
            if (participant.Team == null)
            {
                return Retry(ctx, message: Localization.NoTeamWarning);
            }

            var team = (await Teams.GetWithMembersAsync(participant.TeamId!.Value))!;

            StringBuilder teamInfo = new();
            teamInfo.AppendLine(team.Name);

            foreach (var member in team.Members)
            {
                teamInfo.Append("- ").AppendLine(member.FormatDisplay());
            }

            return Retry(ctx, message: string.Format(Localization.TeamInfo, participant.GetNameOnly(), teamInfo.ToString(), _localizationService.GetString(team.Case.ToString())));
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.ModuleSelectRootMenu), ParentStateName = RootStateName)]
    [MenuItem(nameof(Labels.SelectCase))]
    public async Task<StateResult> OnCaseAsync_HackathonNotStarted(ModuleStateContext ctx)
    {
        if (DateTime.UtcNow > _config.HackathonStart)
            return ToState(nameof(OnCaseAsync_HackathonStarted));
        if (ctx.Matches(Labels.SelectCase))
        {
            var participant = await GetParticipantAsync(ctx);
            if (participant == null)
                return Fail();
            if (participant.Team == null)
                return Completed(Localization.NoTeamWarning);

            var teamsByCase = Teams.GetAll().AsNoTracking().GroupBy(x => x.Case).ToList();
            int countLD = teamsByCase.FirstOrDefault(x => x.Key == Case.LD)?.Count() ?? 0;
            int countTBank = teamsByCase.FirstOrDefault(x => x.Key == Case.TBank)?.Count() ?? 0;

            (bool exceedLD, bool exceedTBank) = ExceedsLimit(countLD, countTBank);

            if (!exceedLD)
                await ctx.ReplyAsync(Localization.LDCaseInfo);
            if (!exceedTBank)
                await ctx.ReplyAsync(Localization.TBankCaseInfo);

            return (exceedLD, exceedTBank) switch
            {
                (true, true) => ToRoot(message: Localization.CaseSelectionUnavailable),
                (true, false) => ToState(nameof(OnSelectCaseAsync_TBankOnly)),
                (false, true) => ToState(nameof(OnSelectCaseAsync_LDOnly)),
                (false, false) => ToState(nameof(OnSelectCaseAsync_FullSelection)),
            };
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.RequestCasePrompt), ParentStateName = nameof(OnCaseAsync_HackathonNotStarted))]
    [MenuRow(nameof(Labels.CaseLD), nameof(Labels.CaseTBank))]
    public async Task<StateResult> OnSelectCaseAsync_FullSelection(ModuleStateContext ctx)
        => await ProcessCaseSelectionAsync(ctx);

    [MenuState(nameof(Localization.RequestCasePrompt), ParentStateName = nameof(OnCaseAsync_HackathonNotStarted))]
    [MenuItem(nameof(Labels.CaseTBank))]
    public async Task<StateResult> OnSelectCaseAsync_TBankOnly(ModuleStateContext ctx)
        => await ProcessCaseSelectionAsync(ctx);

    [MenuState(nameof(Localization.RequestCasePrompt), ParentStateName = nameof(OnCaseAsync_HackathonNotStarted))]
    [MenuItem(nameof(Labels.CaseLD))]
    public async Task<StateResult> OnSelectCaseAsync_LDOnly(ModuleStateContext ctx)
        => await ProcessCaseSelectionAsync(ctx);

    [MenuState(nameof(Localization.ModuleSelectRootMenu), ParentStateName = RootStateName)]
    [MenuItem(nameof(Labels.MySubmission))]
    [MenuItem(nameof(Labels.UploadPresentation))]
    [MenuItem(nameof(Labels.UploadRepo))]
    public async Task<StateResult> OnCaseAsync_HackathonStarted(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.UploadPresentation))
        {
            if (DateTime.UtcNow > _config.StopCode)
            {
                return Completed(Localization.HackathonEndedWarning);
            }
            return ToState(nameof(OnUploadPresentation));
        }
        else if (ctx.Matches(Labels.UploadRepo))
        {
            if (DateTime.UtcNow > _config.StopCode)
            {
                return Completed(Localization.HackathonEndedWarning);
            }
            return ToState(nameof(OnUploadRepo));
        }
        else if (ctx.Matches(Labels.MySubmission))
        {
            var participant = await GetParticipantAsync(ctx);
            if (participant.Team == null)
                return Fail(message: Localization.NoTeamWarning);
            var submission = participant.Team.Submission;

            if (submission == null)
            {
                await ctx.ReplyAsync(Localization.NoPresentationYet);
                await ctx.ReplyAsync(Localization.NoRepoYet);
                return ToRoot();
            }

            if (!string.IsNullOrEmpty(submission.PresentationFileUrl))
            {
                InputFile file = InputFile.FromFileId(submission.PresentationFileUrl);
                await ctx.BotClient.SendDocument(ctx.Chat, file, Localization.PresentationFileInfo.FormatInvariant(participant.Team.Name));
            }
            else if (!string.IsNullOrEmpty(submission.PresentationLink))
            {
                await ctx.ReplyAsync(Localization.PresentationLinkInfo.FormatInvariant(participant.Team.Name, submission.PresentationLink));
            }
            else
            {
                await ctx.ReplyAsync(Localization.NoPresentationYet);
            }

            if (!string.IsNullOrEmpty(submission.RepoUrl))
            {
                await ctx.ReplyAsync(Localization.RepoLinkInfo.FormatInvariant(participant.Team.Name, submission.RepoUrl));
            }
            else
            {
                await ctx.ReplyAsync(Localization.NoRepoYet);
            }

            return ToRoot();
        }
        return InvalidInput(ctx);
    }

    [PromptState<string>(nameof(Localization.UploadPresentationPrompt), AllowFileInput = true)]
    public async Task<StateResult> OnUploadPresentation(PromptStateContext<string> ctx)
    {
        var participant = await GetParticipantAsync(ctx);
        if (participant.Team == null)
            return Fail(message: Localization.NoTeamWarning);
        var submission = participant.Team.Submission;
        if (submission == null)
        {
            submission = new()
            {
                Case = participant.Team.Case,
                Team = participant.Team,
                SubmittedBy = participant,
            };
            await Submissions.AddAsync(submission);
        }

        if (ctx.Message is FileMessageContent file)
        {
            submission.PresentationFileUrl = file.FileId;
            submission.PresentationLink = null;
        }
        else if (ctx.Input.TryGetValue(out string link))
        {
            submission.PresentationLink = link;
            submission.PresentationFileUrl = null;
        }
        else
        {
            return InvalidInput(ctx);
        }

        submission.SubmittedById = participant.Id;
        submission.SubmittedAt = DateTime.UtcNow;
        await Submissions.SaveChangesAsync(ctx.CancellationToken);
        return Back(ctx, message: Localization.PresentationUploaded);
    }

    [PromptState<string>(nameof(Localization.UploadRepoPrompt))]
    public async Task<StateResult> OnUploadRepo(PromptStateContext<string> ctx)
    {
        var participant = await GetParticipantAsync(ctx);
        if (participant.Team == null)
            return Fail(message: Localization.NoTeamWarning);
        var submission = participant.Team.Submission;
        if (submission == null)
        {
            submission = new()
            {
                Case = participant.Team.Case,
                Team = participant.Team,
                SubmittedBy = participant,
            };
            await Submissions.AddAsync(submission, ctx.CancellationToken);
            await Submissions.SaveChangesAsync(ctx.CancellationToken);
        }

        if (ctx.Input.TryGetValue(out string link))
        {
            submission.RepoUrl = link;
        }
        else
        {
            return InvalidInput(ctx);
        }

        submission.SubmittedById = participant.Id;
        submission.SubmittedAt = DateTime.UtcNow;
        await Submissions.SaveChangesAsync(ctx.CancellationToken);
        return Back(ctx, message: Localization.RepoUploaded);
    }

    private async Task<StateResult> ProcessCaseSelectionAsync(ModuleStateContext ctx)
    {
        var participant = await GetParticipantAsync(ctx);
        if (participant == null)
            return Fail();
        if (participant.Team == null)
            return Completed(Localization.NoTeamWarning);

        var teamsByCase = Teams.GetAll().AsNoTracking().GroupBy(x => x.Case).ToList();
        int countLD = teamsByCase.FirstOrDefault(x => x.Key == Case.LD)?.Count() ?? 0;
        int countTBank = teamsByCase.FirstOrDefault(x => x.Key == Case.TBank)?.Count() ?? 0;

        (bool exceedLD, bool exceedTBank) = ExceedsLimit(countLD, countTBank);

        if (ctx.Matches(Labels.CaseLD) && !exceedLD)
        {
            participant.Team.Case = Case.LD;
            await Teams.SaveChangesAsync(ctx.CancellationToken);
            return Completed(Localization.CaseSelectedSuccessfully);
        }
        else if (ctx.Matches(Labels.CaseTBank) && !exceedTBank)
        {
            participant.Team.Case = Case.TBank;
            await Teams.SaveChangesAsync(ctx.CancellationToken);
            return Completed(Localization.CaseSelectedSuccessfully);
        }

        return InvalidInput(ctx);
    }

    private (bool, bool) ExceedsLimit(int v1, int v2) => (v1 >= _config.MaxTeamsPerCase, v2 >= _config.MaxTeamsPerCase);

    private async Task<Participant> GetParticipantAsync(ModuleStateContext ctx) => (await Participants.FindByTelegramIdAsync(ctx.User.TelegramId)) ?? throw new InvalidOperationException();
}