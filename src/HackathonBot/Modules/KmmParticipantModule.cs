using System.Text;
using HackathonBot.Models;
using HackathonBot.Models.Kmm;
using HackathonBot.Properties;
using HackathonBot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyBots.Core.Fsm.States;
using MyBots.Modules.Common;
using MyBots.Modules.Common.Interactivity;
using Quartz.Util;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace HackathonBot.Modules;

internal class KmmParticipantModule(IServiceProvider services) :
    BotModule(Labels.Kmm, [Roles.Participant], services)
{
    private const int DataLeakPrice = 10;
    private const int HackerAttackPrice = 10;
    private const int SlanderPrice = 5;

    private readonly IKmmGameService _kmmService = services.GetRequiredService<IKmmGameService>();
    private readonly KmmConfig _config = services.GetRequiredService<IOptions<KmmConfig>>().Value;

    [MenuRow(nameof(Labels.AboutKmm), nameof(Labels.MyTeam))]
    [MenuItem(nameof(Labels.Vote))]
    [MenuItem(nameof(Labels.Abilities))]
    public override async Task<StateResult> OnModuleRootAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.AboutKmm))
            return Completed(Localization.AboutKmmMessage);

        var team = await _kmmService.GetKmmTeamByTelegramIdAsync(ctx.User.TelegramId);
        if (team == null)
        {
            return FailWithMessage(Localization.KmmNotStarted);
        }

        if (ctx.Matches(Labels.MyTeam))
        {
            int citizensBank = await Banks.GetParticipantCoinsAsync(ctx.CancellationToken);

            string message = Localization.KmmTeamInfo.FormatInvariant(
                team.Color,
                LocalizationService.GetString(team.Role.ToString()),
                team.Score,
                // {3} - mafia bank + team kill state
                (IsMafia(team) ? Localization.MafiaPersonalBankInfo.FormatInvariant(await Banks.GetCoinsAsync(team.Color, ctx.CancellationToken)) : string.Empty) +
                (!team.IsAlive ? Localization.TeamOut + Environment.NewLine + Localization.TeamOutNote : string.Empty),
                citizensBank,
                _config.CitizensBankToWin);
            return Completed(message);
        }
        else if (ctx.Matches(Labels.Vote))
        {
            (DateTime lastVoteStarted, bool isVoteOpen) = await CheckVoteState();

            if (!isVoteOpen)
                return RetryWithMessage(ctx, Localization.VoteClosedWarning);

            var vote = await AbilityUses.GetLastUseAsync(team.Id, Ability.Vote, ctx.CancellationToken);

            string message;

            if (vote == null || vote.TargetTeam == null || vote.UsedAt < lastVoteStarted)
                message = Localization.CurrentVoteDefailsNoVote.FormatInvariant(team.Color);
            else
                message = Localization.CurrentVoteDetails.FormatInvariant(team.Color, vote.TargetTeam.Color);

            var teams = await KmmTeams.GetAliveTeamsAsync(ctx.CancellationToken);

            if (IsMafia(team))
            {
                List<List<KeyboardButton>> buttons = [];

                foreach (var t in teams.Where(x => !IsMafia(x)))
                    buttons.Add([new(t.Color)]);

                await ctx.BotClient.SendMessage(ctx.Chat, message, replyMarkup: new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true });
                return ToState(nameof(OnVoteAsync));
            }
            else
            {
                List<List<KeyboardButton>> buttons = [];

                foreach (var t in teams)
                    buttons.Add([new(t.Color)]);

                await ctx.BotClient.SendMessage(ctx.Chat, message, replyMarkup: new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true });
                return ToState(nameof(OnVoteAsync));
            }
        }
        else if (ctx.Matches(Labels.Abilities))
        {
            switch (team.Role)
            {
                case MafiaRole.Citizen:
                    return ToState(nameof(OnCitizenAbilitySelectedAsync));
                case MafiaRole.Detective:
                    return ToState(nameof(OnDetectiveAbilitySelectedAsync));
                case MafiaRole.Doctor:
                    return ToState(nameof(OnDoctorAbilitySelectedAsync));
                case MafiaRole.Godfather:
                    return ToState(nameof(OnGodfatherAbilitySelectedAsync));
                case MafiaRole.Mafia:
                    return ToState(nameof(OnMafiaAbilitySelectedAsync));
            }
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.SelectAbility))]
    [MenuItem(nameof(Labels.WorkEthic))]
    public async Task<StateResult> OnCitizenAbilitySelectedAsync(ModuleStateContext ctx)
    {
        var team = await _kmmService.GetKmmTeamByTelegramIdAsync(ctx.User.TelegramId);
        if (team == null)
        {
            return FailWithMessage(Localization.KmmNotStarted);
        }

        if (ctx.Matches(Labels.WorkEthic))
        {
            await ctx.ReplyAsync(Localization.WorkEthicDescription);
            var used = (await AbilityUses.GetByAbilityAsync(Ability.WorkEthic, ctx.CancellationToken)).Any(x => x.TeamId == team.Id);
            if (used)
                return Completed(Localization.AbilityUsageLimitReached);

            return ToStateWith(nameof(OnConfirmAbilityUsageAsync), Ability.WorkEthic);
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.SelectAbility))]
    [MenuItem(nameof(Labels.Check))]
    public async Task<StateResult> OnDetectiveAbilitySelectedAsync(ModuleStateContext ctx)
    {
        var team = await _kmmService.GetKmmTeamByTelegramIdAsync(ctx.User.TelegramId);
        if (team == null)
        {
            return FailWithMessage(Localization.KmmNotStarted);
        }

        if (ctx.Matches(Labels.Check))
        {
            await ctx.ReplyAsync(Localization.CheckDescription);
            return ToStateWith(nameof(OnConfirmAbilityUsageAsync), Ability.Check);
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.SelectAbility))]
    [MenuItem(nameof(Labels.Defense))]
    [MenuItem(nameof(Labels.Firewall))]
    public async Task<StateResult> OnDoctorAbilitySelectedAsync(ModuleStateContext ctx)
    {
        var team = await _kmmService.GetKmmTeamByTelegramIdAsync(ctx.User.TelegramId);
        if (team == null)
        {
            return FailWithMessage(Localization.KmmNotStarted);
        }

        if (ctx.Matches(Labels.Defense))
        {
            await ctx.ReplyAsync(Localization.DefenseDescription);
            return ToStateWith(nameof(OnConfirmAbilityUsageAsync), Ability.Defense);
        }
        else if (ctx.Matches(Labels.Firewall))
        {
            await ctx.ReplyAsync(Localization.FirewallDescription);
            var used = (await AbilityUses.GetByAbilityAsync(Ability.Firewall, ctx.CancellationToken)).Any(x => x.TeamId == team.Id);
            if (used)
                return Completed(Localization.AbilityUsageLimitReached);

            return ToStateWith(nameof(OnConfirmAbilityUsageAsync), Ability.Firewall);
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.SelectAbility))]
    [MenuItem(nameof(Labels.Slander))]
    [MenuItem(nameof(Labels.DataLeak))]
    [MenuItem(nameof(Labels.HackerAttack))]
    public async Task<StateResult> OnGodfatherAbilitySelectedAsync(ModuleStateContext ctx)
    {
        var team = await _kmmService.GetKmmTeamByTelegramIdAsync(ctx.User.TelegramId);
        if (team == null)
        {
            return FailWithMessage(Localization.KmmNotStarted);
        }

        int bank = await Banks.GetCoinsAsync(team.Color, ctx.CancellationToken);

        if (ctx.Matches(Labels.Slander))
        {
            await ctx.ReplyAsync(Localization.SlanderDescription);
            if (bank < SlanderPrice)
                return Completed(Localization.NotEnoughCoins);

            return ToStateWith(nameof(OnConfirmAbilityUsageAsync), Ability.Slander);
        }
        else if(ctx.Matches(Labels.DataLeak))
        {
            await ctx.ReplyAsync(Localization.DataLeakDescription);
            if (bank < DataLeakPrice)
                return Completed(Localization.NotEnoughCoins);

            return ToStateWith(nameof(OnConfirmAbilityUsageAsync), Ability.DataLeak);
        }
        else if (ctx.Matches(Labels.HackerAttack))
        {
            await ctx.ReplyAsync(Localization.HackerAttackDescription);
            if (bank < HackerAttackPrice)
                return Completed(Localization.NotEnoughCoins);

            return ToStateWith(nameof(OnConfirmAbilityUsageAsync), Ability.HackerAttack);
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.SelectAbility))]
    [MenuItem(nameof(Labels.DataLeak))]
    [MenuItem(nameof(Labels.HackerAttack))]
    public async Task<StateResult> OnMafiaAbilitySelectedAsync(ModuleStateContext ctx)
    {
        var team = await _kmmService.GetKmmTeamByTelegramIdAsync(ctx.User.TelegramId);
        if (team == null)
        {
            return FailWithMessage(Localization.KmmNotStarted);
        }

        int bank = await Banks.GetCoinsAsync(team.Color, ctx.CancellationToken);

        if (ctx.Matches(Labels.DataLeak))
        {
            await ctx.ReplyAsync(Localization.DataLeakDescription);
            if (bank < DataLeakPrice)
                return Completed(Localization.NotEnoughCoins);

            return ToStateWith(nameof(OnConfirmAbilityUsageAsync), Ability.DataLeak);
        }
        else if (ctx.Matches(Labels.HackerAttack))
        {
            await ctx.ReplyAsync(Localization.HackerAttackDescription);
            if (bank < HackerAttackPrice)
                return Completed(Localization.NotEnoughCoins);

            return ToStateWith(nameof(OnConfirmAbilityUsageAsync), Ability.HackerAttack);
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.ConfirmAbilityUsage), BackButton = false)]
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public async Task<StateResult> OnConfirmAbilityUsageAsync(ModuleStateContext ctx)
    {
        var team = await _kmmService.GetKmmTeamByTelegramIdAsync(ctx.User.TelegramId);
        if (team == null)
        {
            return FailWithMessage(Localization.KmmNotStarted);
        }

        if (!ctx.TryGetData(out Ability ability))
            return Fail();

        if (ctx.Matches(Labels.Yes))
        {
            switch (ability)
            {
                case Ability.Check:
                case Ability.Defense:
                case Ability.Slander:
                    {
                        StringBuilder liveTeams = new();
                        liveTeams.AppendLine(Localization.LiveTeams);

                        List<List<KeyboardButton>> targetTeams = [];

                        foreach (var t in await KmmTeams.GetAliveTeamsAsync(ctx.CancellationToken))
                        {
                            liveTeams.Append("- ").AppendLine(t.Color);
                            targetTeams.Add([new(t.Color)]);
                        }

                        await ctx.BotClient.SendMessage(ctx.Chat, liveTeams.ToString(), replyMarkup: new ReplyKeyboardMarkup(targetTeams));
                        return ToStateWith(nameof(OnRequestAbilityTargetAsync), ability);
                    }
                case Ability.DataLeak:
                    {
                        int coins = await Banks.GetParticipantCoinsAsync(ctx.CancellationToken);
                        if (coins < 10)
                            return FailWithMessage(Localization.BankIsEmpty);
                        var error = await TryBuyAsync(ctx, team, DataLeakPrice);
                        if (error != null)
                            return error;
                        await Banks.SetParticipantCoinsAsync(coins - 10, ctx.CancellationToken);
                        break;
                    }
                case Ability.HackerAttack:
                    {
                        var error = await TryBuyAsync(ctx, team, HackerAttackPrice);
                        if (error != null)
                            return error;
                        break;
                    }
            }

            var use = new AbilityUse()
            {
                Ability = ability,
                TeamId = team.Id,
                UsedAt = DateTime.UtcNow,
            };

            await AbilityUses.AddAsync(use, ctx.CancellationToken);
            await AbilityUses.SaveChangesAsync();
            return Completed(Localization.AbilityUsed);
        }
        return ToRoot();
    }

    [MenuState(nameof(Localization.SelectTeam))]
    [InheritKeyboard]
    public async Task<StateResult> OnRequestAbilityTargetAsync(ModuleStateContext ctx)
    {
        var team = await _kmmService.GetKmmTeamByTelegramIdAsync(ctx.User.TelegramId);
        if (team == null)
        {
            return FailWithMessage(Localization.KmmNotStarted);
        }

        if (!ctx.TryGetData(out Ability ability))
            return Fail();

        if (ctx.Message is not TextMessageContent msg)
            return InvalidInput(ctx);

        var target = await KmmTeams.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Color == msg.Text);
        if (target == null)
            return InvalidInput(ctx);

        if (ability == Ability.Slander)
        {
            var error = await TryBuyAsync(ctx, team, SlanderPrice);
            if (error != null)
                return error;
        }

        var use = new AbilityUse()
        {
            Ability = ability,
            TeamId = team.Id,
            TargetTeamId = target.Id,
            UsedAt = DateTime.UtcNow,
        };
        await AbilityUses.AddAsync(use);
        await AbilityUses.SaveChangesAsync();
        return Completed(Localization.AbilityUsed);
    }

    private async Task<StateResult?> TryBuyAsync(ModuleStateContext ctx, KmmTeam team, int price)
    {
        int bank = await Banks.GetCoinsAsync(team.Color, ctx.CancellationToken);
        if (bank < price)
            return  FailWithMessage(Localization.NotEnoughCoins);
        await Banks.SetCoinsAsync(team.Color, bank - price, ctx.CancellationToken);
        return null;
    }

    [MenuState(nameof(Localization.VoteTargetRequest))]
    [InheritKeyboard]
    public async Task<StateResult> OnVoteAsync(ModuleStateContext ctx)
    {
        var team = await _kmmService.GetKmmTeamByTelegramIdAsync(ctx.User.TelegramId);
        if (team == null)
        {
            return FailWithMessage(Localization.KmmNotStarted);
        }

        (DateTime lastVoteStarted, bool isVoteOpen) = await CheckVoteState();

        if (!isVoteOpen)
            return RetryWithMessage(ctx, Localization.VoteClosedWarning);

        if (ctx.Message is TextMessageContent msg)
        {
            var target = (await KmmTeams.GetAliveTeamsAsync(ctx.CancellationToken)).FirstOrDefault(x => x.Color == msg.Text);
            if (target == null || IsMafia(target) && IsMafia(team))
                return InvalidInput(ctx);

            var vote = new AbilityUse()
            {
                Ability = Ability.Vote,
                TeamId = team.Id,
                TargetTeamId = target.Id,
                UsedAt = DateTime.UtcNow,
            };
            await AbilityUses.AddAsync(vote, ctx.CancellationToken);
            await AbilityUses.SaveChangesAsync(ctx.CancellationToken);
            return Completed(Localization.VoteAccepted);
        }

        return InvalidInput(ctx);
    }

    private async Task<(DateTime lastVoteStarted, bool isVoteOpen)> CheckVoteState()
    {
        var votesStarted = await Audit.GetByTypeAsync(EventType.VoteStarted);
        var votesEnded = await Audit.GetByTypeAsync(EventType.VoteEnded);

        var lastVoteStarted = votesStarted.OrderByDescending(x => x.LoggedAt).FirstOrDefault()?.LoggedAt ?? DateTime.MinValue;
        var lastVoteEnded = votesEnded.OrderByDescending(x => x.LoggedAt).FirstOrDefault()?.LoggedAt ?? DateTime.MinValue;

        // If vote is started but not ended
        bool isVoteOpen = lastVoteStarted > lastVoteEnded;
        return (lastVoteStarted, isVoteOpen);
    }

    private static bool IsMafia(KmmTeam team) => team.Role is MafiaRole.Mafia or MafiaRole.Godfather;
}