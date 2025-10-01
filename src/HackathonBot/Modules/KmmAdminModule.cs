using System.Text;
using HackathonBot.Properties;
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

internal class KmmAdminModule(IServiceProvider services) : BotModule(Labels.KmmManagement, [Roles.Organizer, Roles.Admin], services)
{
    private readonly KmmConfig _config = services.GetRequiredService<IOptions<KmmConfig>>().Value;

    [MenuItem(nameof(Labels.KmmTeamsStatus))]
    [MenuItem(nameof(Labels.KmmCreateEvent))]
    [MenuItem(nameof(Labels.KmmManageEvents))]
    [MenuItem(nameof(Labels.KmmStartVote))]
    [MenuItem(nameof(Labels.KmmAuditAbilities))]
    [MenuItem(nameof(Labels.InitializeKmm))]
    public override async Task<StateResult> OnModuleRootAsync(ModuleStateContext ctx)
    {
        bool isInitialized = await KmmTeams.GetAll().AnyAsync(ctx.CancellationToken);

        if (ctx.Matches(Labels.KmmTeamsStatus))
        {
            if (!isInitialized)
                return Retry(ctx, message: Localization.KmmNotInitialized);

            StringBuilder details = new();
            details.AppendLine(Localization.TeamsList);

            foreach (var kmmTeam in KmmTeams.GetAll().AsNoTracking().Include(x => x.HackathonTeam))
            {
                details.AppendLine(Localization.KmmTeamDetails.FormatInvariant(
                    kmmTeam.HackathonTeam.Name,
                    _localizationService.GetString(kmmTeam.Role.ToString()),
                    kmmTeam.IsAlive.AsEmoji().ToUnicode(),
                    kmmTeam.Score))
                    .AppendLine();
            }
            await ctx.ReplyAsync(details.ToString());
            return Completed(Localization.UploadCompleted);
        }
        else if (ctx.Matches(Labels.KmmCreateEvent))
        {
            return ToState(nameof(OnRequestEventNameAsync));
        }
        else if (ctx.Matches(Labels.KmmManageEvents))
        {
            StringBuilder events = new();
            events.AppendLine(Localization.EventsList);
            List<List<KeyboardButton>> keyboard = []; 

            foreach (var ev in Events.GetAll().AsNoTracking())
            {
                events.Append("- ").AppendLine(ev.Name);
                keyboard.Add([new(ev.Name)]);
            }

            await ctx.BotClient.SendMessage(ctx.Chat, events.ToString(), replyMarkup: new ReplyKeyboardMarkup(keyboard));
            return ToState(nameof(OnSelectEventAsync));
        }
        else if (ctx.Matches(Labels.KmmAuditAbilities))
        {
            StringBuilder events = new();
            events.AppendLine(Localization.EventsList);

            foreach (var use in AbilityUses.GetAll().AsNoTracking().Include(x => x.Team).ThenInclude(t => t.HackathonTeam).OrderByDescending(x => x.UsedAt).Take(100))
            {
                events.AppendLine($"[{use.UsedAt.AddHours(_config.TimeOffset)}] {use.Team.HackathonTeam.Name} – {use.Ability}");
            }

            await ctx.ReplyAsync(events.ToString());
            return ToRoot();
        }
        else if (ctx.Matches(Labels.KmmStartVote))
        {
            if (!isInitialized)
                return Retry(ctx, message: Localization.KmmNotInitialized);
            // TODO
        }
        else if (ctx.Matches(Labels.InitializeKmm))
        {
            // TODO
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.SelectEvent))]
    [InheritKeyboard]
    public async Task<StateResult> OnSelectEventAsync(ModuleStateContext ctx)
    {
        if (ctx.Message is not TextMessageContent msg)
            return InvalidInput(ctx);

        var ev = await Events.FindByNameAsync(msg.Text, ctx.CancellationToken);
        if (ev == null)
            return InvalidInput(ctx);

        return ToState(nameof(OnManageEventAsync), ev.Id.ToString());
    }

    [MenuState(nameof(Localization.ModuleSelectRootMenu))]
    [MenuItem(nameof(Labels.StartEvent))]
    [MenuItem(nameof(Labels.KmmCreateQuest))]
    [MenuItem(nameof(Labels.KmmManageQuests))]
    [MenuItem(nameof(Labels.DeleteEvent))]
    public async Task<StateResult> OnManageEventAsync(ModuleStateContext ctx)
    {
        if (!long.TryParse(ctx.StateData, out long eventId))
            return Fail();

        var ev = await Events.GetByIdWithQuestsAsync(eventId, ctx.CancellationToken);
        if (ev == null)
            return Fail();

        if (ctx.Matches(Labels.StartEvent))
        {
            // TODO
        }
        else if (ctx.Matches(Labels.KmmCreateQuest))
        {
            return ToState(nameof(OnInputQuestNameAsync), ctx.StateData);
        }
        else if (ctx.Matches(Labels.KmmManageQuests))
        {
            var normalQuests = ev.Quests.Where(q => !q.IsSabotage).ToList();
            var sabotageQuests = ev.Quests.Where(q => q.IsSabotage).ToList();

            StringBuilder normalText = new();
            normalText.AppendLine(Localization.NormalQuestsList);
            if (normalQuests.Count == 0)
                normalText.AppendLine(Localization.NoQuests);
            else
            {
                foreach (var q in normalQuests)
                    normalText.AppendLine($"- {q.Name}").AppendLine($"> {q.Description}");
            }
            await ctx.ReplyAsync(normalText.ToString());

            StringBuilder sabotageText = new();
            sabotageText.AppendLine(Localization.SabotageQuestsList);
            if (sabotageQuests.Count == 0)
                sabotageText.AppendLine(Localization.NoQuests);
            else
            {
                foreach (var q in sabotageQuests)
                    sabotageText.AppendLine($"- {q.Name}").AppendLine($"> {q.Description}");
            }

            List<List<KeyboardButton>> keyboard = [];
            foreach (var q in ev.Quests)
            {
                keyboard.Add([new(q.Name)]);
            }

            await ctx.BotClient.SendMessage(ctx.Chat, sabotageText.ToString(), replyMarkup: new ReplyKeyboardMarkup(keyboard) { ResizeKeyboard = true });
            return ToState(nameof(OnSelectQuestToManageAsync), ctx.StateData);
        }
        else if (ctx.Matches(Labels.KmmAuditAbilities))
        {

        }
        else if (ctx.Matches(Labels.DeleteEvent))
        {
            return ToState(nameof(OnConfirmDeleteEventAsync), ctx.StateData);
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.SelectQuest))]
    [InheritKeyboard]
    public async Task<StateResult> OnSelectQuestToManageAsync(ModuleStateContext ctx)
    {
        if (ctx.Message is not TextMessageContent msg)
            return InvalidInput(ctx);

        if (!long.TryParse(ctx.StateData, out long eventId))
            return Fail();

        var quest = await Quests.FindByNameAsync(eventId, msg.Text, ctx.CancellationToken);
        if (quest == null)
            return InvalidInput(ctx);

        return ToState(nameof(OnQuestManageMenuAsync), $"{eventId}{Environment.NewLine}{quest.Id}");
    }

    [MenuState(nameof(Localization.ModuleSelectRootMenu), ParentStateName = nameof(OnManageEventAsync))]
    [MenuItem(nameof(Labels.EditQuestName))]
    [MenuItem(nameof(Labels.EditQuestDescription))]
    [MenuItem(nameof(Labels.DeleteQuest))]
    public async Task<StateResult> OnQuestManageMenuAsync(ModuleStateContext ctx)
    {
        var parts = ctx.StateData.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length < 2)
            return Fail();

        if (!long.TryParse(parts[0], out long eventId))
            return Fail();
        if (!long.TryParse(parts[1], out long questId))
            return Fail();

        var quest = await Quests.GetByIdAsync(questId, ctx.CancellationToken);
        if (quest == null)
            return Fail();

        if (ctx.Matches(Labels.EditQuestName))
        {
            return ToState(nameof(OnPromptEditQuestNameAsync), ctx.StateData);
        }
        else if (ctx.Matches(Labels.EditQuestDescription))
        {
            return ToState(nameof(OnPromptEditQuestDescriptionAsync), ctx.StateData);
        }
        else if (ctx.Matches(Labels.DeleteQuest))
        {
            return ToState(nameof(OnConfirmDeleteQuestAsync), ctx.StateData);
        }

        return InvalidInput(ctx);
    }

    [PromptState<string>(nameof(Localization.InputNewQuestName))]
    public async Task<StateResult> OnPromptEditQuestNameAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.Input.TryGetValue(out string newName))
            return InvalidInput(ctx);

        var parts = ctx.StateData.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length < 2) return Fail();
        if (!long.TryParse(parts[1], out long questId)) return Fail();

        var quest = await Quests.GetByIdAsync(questId, ctx.CancellationToken);
        if (quest == null) return Fail();

        if (await Quests.FindByNameAsync(quest.EventId, newName, ctx.CancellationToken) != null)
            return Retry(ctx, message: Localization.NameTaken);

        quest.Name = newName;
        await Quests.SaveChangesAsync(ctx.CancellationToken);
        return Completed(Localization.QuestUpdated);
    }

    [PromptState<string>(nameof(Localization.InputNewQuestDescription))]
    public async Task<StateResult> OnPromptEditQuestDescriptionAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.Input.TryGetValue(out string newDescription))
            return InvalidInput(ctx);

        var parts = ctx.StateData.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length < 2) return Fail();
        if (!long.TryParse(parts[1], out long questId)) return Fail();

        var quest = await Quests.GetByIdAsync(questId, ctx.CancellationToken);
        if (quest == null) return Fail();

        quest.Description = newDescription;
        await Quests.SaveChangesAsync(ctx.CancellationToken);
        return Completed(Localization.QuestUpdated);
    }

    [MenuState(nameof(Localization.ConfirmQuestDeletion))]
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public async Task<StateResult> OnConfirmDeleteQuestAsync(ModuleStateContext ctx)
    {
        var parts = ctx.StateData.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length < 2)
            return Fail();
        if (!long.TryParse(parts[1], out long questId))
            return Fail();

        if (ctx.Matches(Labels.Yes))
        {
            var quest = await Quests.GetByIdAsync(questId, ctx.CancellationToken);
            if (quest == null) return Fail();

            await Quests.DeleteAsync(quest, ctx.CancellationToken);
            await Quests.SaveChangesAsync(ctx.CancellationToken);
            return Completed(Localization.QuestDeleted);
        }
        else if (ctx.Matches(Labels.No))
        {
            return Completed(Localization.Cancel);
        }

        return InvalidInput(ctx);
    }

    [PromptState<string>(nameof(Localization.InputQuestName))]
    public async Task<StateResult> OnInputQuestNameAsync(PromptStateContext<string> ctx)
    {
        if (!long.TryParse(ctx.StateData, out long eventId))
            return Fail();

        if (!ctx.Input.TryGetValue(out string name))
            return InvalidInput(ctx);
        var quest = await Quests.FindByNameAsync(eventId, name, ctx.CancellationToken);
        if (quest != null)
            return InvalidInput(ctx);
        return ToState(nameof(OnInputQuestDescriptionAsync), ctx.StateData + Environment.NewLine + name);
    }

    [PromptState<string>(nameof(Localization.InputQuestDescription))]
    public Task<StateResult> OnInputQuestDescriptionAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.Input.TryGetValue(out string description))
            return Task.FromResult(InvalidInput(ctx));
        return Task.FromResult(ToState(nameof(OnSelectQuestIsSabotageAsync), ctx.StateData + Environment.NewLine + description));
    }

    [MenuState(nameof(Localization.SelectQuestIsSabotage))]
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public async Task<StateResult> OnSelectQuestIsSabotageAsync(ModuleStateContext ctx)
    {
        bool isSabotage;
        if (ctx.Matches(Labels.Yes)) isSabotage = true;
        else if (ctx.Matches(Labels.No)) isSabotage = false;
        else return InvalidInput(ctx);

        string[] parts = ctx.StateData.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (!long.TryParse(parts[0], out long eventId))
            return Fail();

        string name = parts[1];
        string description = parts[2];

        var quest = await Quests.FindByNameAsync(eventId, name, ctx.CancellationToken);
        if (quest != null)
            return Fail();

        quest = new()
        {
            EventId = eventId,
            Name = name,
            Description = description,
            IsSabotage = isSabotage,
        };
        await Quests.AddAsync(quest, ctx.CancellationToken);
        await Quests.SaveChangesAsync(ctx.CancellationToken);
        return ToState(nameof(OnManageEventAsync), eventId.ToString(), message: Localization.QuestCreatedSuccessfully);
    }

    [MenuState(nameof(Localization.ConfirmEventDeletion))]
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public async Task<StateResult> OnConfirmDeleteEventAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.Yes))
        {
            if (!long.TryParse(ctx.StateData, out long eventId))
                return Fail();

            var ev = await Events.GetByIdWithQuestsAsync(eventId, ctx.CancellationToken);
            if (ev == null)
                return Fail();

            await Events.DeleteAsync(ev, ctx.CancellationToken);
            await Events.SaveChangesAsync(ctx.CancellationToken);
            return Completed(Localization.EventDeleted);
        }
        else if (ctx.Matches(Labels.No))
        {
            return Completed(Localization.Cancel);
        }
        return InvalidInput(ctx);
    }

    [PromptState<string>(nameof(Localization.InputEventName))]
    public async Task<StateResult> OnRequestEventNameAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.Input.TryGetValue(out string name))
            return InvalidInput(ctx);

        var ev = await Events.FindByNameAsync(name, ctx.CancellationToken);
        if (ev != null)
            return Retry(ctx, message: Localization.NameTaken);

        return ToState(nameof(OnRequestFirstPlaceRewardAsync), name);
    }

    [PromptState<int>(nameof(Localization.InputFirstPlaceReward))]
    public Task<StateResult> OnRequestFirstPlaceRewardAsync(PromptStateContext<int> ctx)
    {
        if (!ctx.Input.TryGetValue(out int firstPlace))
            return Task.FromResult(InvalidInput(ctx));
        return Task.FromResult(ToState(nameof(OnRequestSecondPlaceRewardAsync), ctx.StateData + Environment.NewLine + firstPlace));
    }

    [PromptState<int>(nameof(Localization.InputSecondPlaceReward))]
    public Task<StateResult> OnRequestSecondPlaceRewardAsync(PromptStateContext<int> ctx)
    {
        if (!ctx.Input.TryGetValue(out int secondPlace))
            return Task.FromResult(InvalidInput(ctx));
        return Task.FromResult(ToState(nameof(OnRequestThirdPlaceRewardAsync), ctx.StateData + Environment.NewLine + secondPlace));
    }

    [PromptState<int>(nameof(Localization.InputThirdPlaceReward))]
    public async Task<StateResult> OnRequestThirdPlaceRewardAsync(PromptStateContext<int> ctx)
    {
        if (!ctx.Input.TryGetValue(out int thirdPlace))
            return InvalidInput(ctx);
        var parts = ctx.StateData.Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        string name = parts[0];
        int firstPlace = int.Parse(parts[1]);
        int secondPlace = int.Parse(parts[2]);

        var ev = await Events.FindByNameAsync(name, ctx.CancellationToken);
        if (ev != null)
            return Fail(message: Localization.NameTaken);

        ev = new()
        {
            Name = name,
            FirstPlaceReward = firstPlace,
            SecondPlaceReward = secondPlace,
            ThirdPlaceReward = thirdPlace,
        };
        await Events.AddAsync(ev, ctx.CancellationToken);
        await Events.SaveChangesAsync(ctx.CancellationToken);
        return Completed(Localization.EventCreated);
    }
}