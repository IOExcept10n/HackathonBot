using System.ComponentModel.DataAnnotations;
using System.Text;
using HackathonBot.Models;
using HackathonBot.Models.Kmm;
using HackathonBot.Properties;
using HackathonBot.Repository.Validation;
using HackathonBot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyBots.Core.Fsm.States;
using MyBots.Modules.Common;
using MyBots.Modules.Common.Interactivity;
using Quartz.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HackathonBot.Modules;

internal class KmmAdminModule(IServiceProvider services) : BotModule(Labels.KmmManagement, [Roles.Organizer, Roles.Admin], services)
{
    private readonly IKmmGameService _kmmService = services.GetRequiredService<IKmmGameService>();
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
                return RetryWithMessage(ctx, message: Localization.KmmNotInitialized);

            StringBuilder details = new();
            details.AppendLine(Localization.TeamsList);

            foreach (var kmmTeam in KmmTeams.GetAll().AsNoTracking().Include(x => x.HackathonTeam))
            {
                details.AppendLine(Localization.KmmTeamDetails.FormatInvariant(
                    kmmTeam.HackathonTeam.Name,
                    LocalizationService.GetString(kmmTeam.Role.ToString()),
                    kmmTeam.IsAlive.AsEmoji().ToUnicode(),
                    kmmTeam.Score))
                    .AppendLine();
            }
            await ctx.ReplyAsync(details.ToString());
            return Completed(Localization.UploadCompleted);
        }
        else if (ctx.Matches(Labels.KmmCreateEvent))
        {
            return ToState(nameof(OnInputEventModelAsync));
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

            foreach (var use in AbilityUses.GetAll().AsNoTracking().Include(x => x.Team).ThenInclude(t => t.HackathonTeam).Include(x => x.TargetTeam).ThenInclude(t => t!.HackathonTeam).OrderByDescending(x => x.UsedAt).Take(100))
            {
                events.AppendLine($"[{use.UsedAt.AddHours(_config.TimeOffset)}] {use.Team.HackathonTeam.Name} – {use.Ability} > {use.TargetTeam?.HackathonTeam.Name}");
            }

            await ctx.ReplyAsync(events.ToString());
            return ToRoot();
        }
        else if (ctx.Matches(Labels.KmmStartVote))
        {
            if (!isInitialized)
                return RetryWithMessage(ctx, message: Localization.KmmNotInitialized);

            // 1. Send audit VoteStarted
            await Audit.AddAsync(new EventAuditEntry
            {
                EventType = EventType.VoteStarted,
                InitiatorId = ctx.User.Id,
                LoggedAt = DateTime.UtcNow
            }, ctx.CancellationToken);
            await Audit.SaveChangesAsync(ctx.CancellationToken);

            // 2. Sending a message to live teams asking them to participate in the voting
            var alive = KmmTeams.GetAll().Include(x => x.HackathonTeam).ThenInclude(t => t.Members).Where(t => t.IsAlive).ToList();
            foreach (var kt in alive)
            {
                var members = kt.HackathonTeam.Members;
                var text = Localization.VoteStartedMessage;
                foreach (var m in members)
                {
                    if (m.TelegramId == 0 || m.TelegramId is not long tgId) continue;
                    try
                    {
                        await ctx.BotClient.SendMessage(new ChatId(tgId), text);
                    }
                    catch { }
                }
            }

            // 3. Put the administrator in a state of waiting for the end of voting
            return ToState(nameof(OnAwaitVoteEndAsync));
        }
        else if (ctx.Matches(Labels.InitializeKmm))
        {
            return ToState(nameof(OnConfirmInitializeKmmAsync));
        }
        return InvalidInput(ctx);
    }

    [MenuState(nameof(Localization.ConfirmInitializeKmm), BackButton = false)]
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public Task<StateResult> OnConfirmInitializeKmmAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.Yes)) return Task.FromResult(ToState(nameof(OnPromptInitializeKmmAsync)));
        return Task.FromResult(ToRoot());
    }

    [PromptState<Unit>(nameof(Localization.UploadTeamsPrompt), AllowFileInput = true, AllowTextInput = false)]
    public async Task<StateResult> OnPromptInitializeKmmAsync(PromptStateContext<Unit> ctx)
    {
        if (ctx.Message is not FileMessageContent file)
            return InvalidInput(ctx);

        using var stream = new MemoryStream();
        await ctx.BotClient.GetInfoAndDownloadFile(file.FileId, stream, ctx.CancellationToken);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        // Skip header
        string? line = await reader.ReadLineAsync();
        List<Team> teams = [];
        List<string> colors = [];

        while ((line = await reader.ReadLineAsync()) != null)
        {
            string[] parts = line.Split(';');
            string teamName = parts[0];
            string color = parts[1];

            var team = await Teams.FindByNameAsync(teamName, ctx.CancellationToken);
            if (team == null)
                continue;

            teams.Add(team);
            colors.Add(color);
        }

        await _kmmService.InitializeGameAsync(teams, _config, ctx.CancellationToken);
        await Teams.SaveChangesAsync(ctx.CancellationToken);
        
        for (int i = 0; i < teams.Count; i++)
        {
            var kmm = await KmmTeams.GetByIdAsync(teams[i].KmmId);
            kmm!.Color = colors[i];
        }
        await KmmTeams.SaveChangesAsync(ctx.CancellationToken);

        return Completed(Localization.KmmInitializedSuccessfully);
    }

    [MenuState(nameof(Localization.AwaitVoteEnd))]
    [MenuRow(nameof(Labels.ConcludeVote))]
    public async Task<StateResult> OnAwaitVoteEndAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.ConcludeVote))
        {
            // 1. Simulate day via KmmService.SimulateDayAsync and get the result
            var dayResult = await _kmmService.SimulateDayAsync(ctx.CancellationToken);

            // 2. Write audit VoteEnded
            await Audit.AddAsync(new EventAuditEntry
            {
                EventType = EventType.VoteEnded,
                InitiatorId = ctx.User.Id,
                LoggedAt = DateTime.UtcNow
            }, ctx.CancellationToken);
            await Audit.SaveChangesAsync(ctx.CancellationToken);

            // 3. Collect the results and send to all teams.
            var msg = new StringBuilder();
            msg.AppendLine(Localization.VoteEndedSuccessfully);
            if (dayResult.LynchedByCitizens != null)
                msg.AppendLine(Localization.LynchedResult.FormatInvariant(dayResult.LynchedByCitizens.Color));
            else
                msg.AppendLine(Localization.LynchedResultNone);

            if (dayResult.KilledByMafia != null)
                msg.AppendLine(Localization.MafiaKillResult.FormatInvariant(dayResult.KilledByMafia.Color));
            else
                msg.AppendLine(Localization.MafiaKillResultNone);

            var detectiveCheck = new StringBuilder();

            if (dayResult.DetectiveCheckResult != null)
            {
                var (teamChecked, isMafia) = dayResult.DetectiveCheckResult;
                if (teamChecked != null)
                {
                    var resText = isMafia.HasValue ? (isMafia.Value ? Localization.DetectiveFoundMafia : Localization.DetectiveFoundCitizen) : Localization.DetectiveCheckBlocked;
                    detectiveCheck.AppendLine(Localization.DetectiveCheckResult.FormatInvariant(teamChecked.Color, resText));
                }
            }

            // Send to everyone
            var allTeams = KmmTeams.GetAll().Include(x => x.HackathonTeam).ThenInclude(t => t.Members).ToList();
            foreach (var kt in allTeams)
            {
                foreach (var m in kt.HackathonTeam.Members)
                {
                    string message = msg.ToString();
                    if (kt.Role == MafiaRole.Detective)
                        message += Environment.NewLine + detectiveCheck.ToString();

                    if (m.TelegramId == 0 || m.TelegramId is not long tgId) continue;
                    try
                    {
                        await ctx.BotClient.SendMessage(new ChatId(tgId), message);
                    }
                    catch { }
                }
            }

            await ctx.ReplyAsync(msg.ToString() + Environment.NewLine + detectiveCheck.ToString());

            return Completed(Localization.VoteEndedSuccessfully);
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

        return ToStateWith(nameof(OnManageEventAsync), ev.Id);
    }

    [MenuState(nameof(Localization.ModuleSelectRootMenu))]
    [MenuItem(nameof(Labels.StartEvent))]
    [MenuItem(nameof(Labels.KmmCreateQuest))]
    [MenuItem(nameof(Labels.ListTeamQuests))]
    [MenuItem(nameof(Labels.KmmManageQuests))]
    [MenuItem(nameof(Labels.DeleteEvent))]
    public async Task<StateResult> OnManageEventAsync(ModuleStateContext ctx)
    {
        if (!ctx.TryGetData(out long eventId))
            return Fail();

        var ev = await Events.GetByIdWithQuestsAsync(eventId, ctx.CancellationToken);
        if (ev == null)
            return Fail();

        if (ctx.Matches(Labels.StartEvent))
        {
            // Start the selected event: distribute quests and enter the event management state.
            // ev is already loaded above in the ev variable

            // 1. Gather teams that have KmmTeam (meaning the game is running for them)
            var teamsWithKmm = Teams.GetAll().Include(t => t.KmmTeam).Where(t => t.KmmId != null).ToList();
            if (teamsWithKmm.Count == 0)
            {
                return RetryWithMessage(ctx, message: Localization.NoTeamsForEvent);
            }

            // 2. Divide available quests into sabotage and regular ones
            var normalQuests = ev.Quests.Where(q => !q.IsSabotage).ToList();
            var sabotageQuests = ev.Quests.Where(q => q.IsSabotage).ToList();

            if (normalQuests.Count == 0 || sabotageQuests.Count == 0)
                return RetryWithMessage(ctx, message: Localization.EventHasNoQuests);

            // 3. Randomly distribute quests: mafia gets only IsSabotage==true, villagers only false.
            var rnd = new Random();
            var assignments = new List<(Team team, Quest quest)>();

            foreach (var team in teamsWithKmm)
            {
                // Let's protect ourselves from the absence of KmmTeam
                var kmm = team.KmmTeam;
                if (kmm == null) continue;

                Quest? chosen = null;
                if (kmm.Role == MafiaRole.Mafia || kmm.Role == MafiaRole.Godfather)
                {
                    if (sabotageQuests.Count > 0)
                        chosen = sabotageQuests[rnd.Next(sabotageQuests.Count)];
                    else if (normalQuests.Count > 0)
                        chosen = normalQuests[rnd.Next(normalQuests.Count)]; // fallback
                }
                else
                {
                    if (normalQuests.Count > 0)
                        chosen = normalQuests[rnd.Next(normalQuests.Count)];
                    else if (sabotageQuests.Count > 0)
                        chosen = sabotageQuests[rnd.Next(sabotageQuests.Count)]; // fallback
                }

                if (chosen != null)
                    assignments.Add((team, chosen));
            }

            // 4. Save an EventEntry for each command (if required in your domain). Use the EventEntry repository.
            foreach (var (team, quest) in assignments)
            {
                var entry = new EventEntry
                {
                    QuestId = quest.Id,
                    KmmTeamId = team.KmmTeam!.Id,
                };
                await EventEntries.AddAsync(entry, ctx.CancellationToken);
            }
            await EventEntries.SaveChangesAsync(ctx.CancellationToken);

            // 5. Send the quest description to teams (only to living team members). We assume Localization and BotClient are available.
            foreach (var (team, quest) in assignments)
            {
                var members = team.Members;
                var text = Localization.EventAssignedQuest.FormatInvariant(ev.Name, team.KmmTeam!.Color, quest.Name, quest.Description);
                foreach (var member in members)
                {
                    if (member.TelegramId == 0 || member.TelegramId is not long tgId) continue;
                    try
                    {
                        await ctx.BotClient.SendMessage(new ChatId(tgId), text);
                    }
                    catch { }
                }
            }

            // 6. Write audit EventStarted
            await Audit.AddAsync(new EventAuditEntry
            {
                EventType = EventType.EventStarted,
                InitiatorId = ctx.User.Id,
                LoggedAt = DateTime.UtcNow,
                Comment = ev.Name,
            }, ctx.CancellationToken);
            await Audit.SaveChangesAsync(ctx.CancellationToken);

            // 7. Put the administrator in the event management state (e.g., OnManageRunningEventAsync),
            // passing the event ID to complete and select 1/2/3 seats.
            return ToStateWith(nameof(OnManageRunningEventAsync), ev.Id, message: Localization.EventStartedSuccessfully);
        }
        else if (ctx.Matches(Labels.KmmCreateQuest))
        {
            return ToStateWith(nameof(OnInputQuestNameAsync), eventId);
        }
        else if (ctx.Matches(Labels.ListTeamQuests))
        {
            StringBuilder list = new();
            list.AppendLine(Localization.TeamsList);
            foreach (var entry in EventEntries.GetAll().AsNoTracking().Include(x => x.Team).Include(x => x.Quest).Where(x => x.Quest.EventId == ev.Id))
            {
                list.Append("- ").Append(entry.Team.Color).Append(": ").Append(entry.Quest.Name).Append(" - ").AppendLine(entry.Quest.Description);
            }
            await ctx.ReplyAsync(list.ToString());
            return ToRoot();
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
            return ToStateWith(nameof(OnSelectQuestToManageAsync), eventId);
        }
        else if (ctx.Matches(Labels.DeleteEvent))
        {
            return ToStateWith(nameof(OnConfirmDeleteEventAsync), eventId);
        }
        return InvalidInput(ctx);
    }

    // New state transfer records
    private readonly record struct EventWinnersSelectionState(
        long EventId,
        List<Guid> RemainingTeamIds,
        List<Guid> ProcessedTeamIds // teams we've already asked about (in order)
    );

    // Record for team selection step
    private readonly record struct TeamSelectionHolder(
        EventWinnersSelectionState State,
        Guid PickedTeamId
    );

    // Record for place assignment step  
    private readonly record struct PlaceAssignmentHolder(
        EventWinnersSelectionState State,
        Guid PickedTeamId,
        int Place
    );

    // Update existing OnManageRunningEventAsync to initialize new state structure
    [MenuState(nameof(Localization.KmmManageRunningEvent))]
    [MenuItem(nameof(Labels.EndEvent))]
    public async Task<StateResult> OnManageRunningEventAsync(ModuleStateContext ctx)
    {
        if (!ctx.TryGetData(out long eventId)) return Fail();

        var ev = await Events.GetByIdAsync(eventId);
        if (ev == null) return Fail();

        if (ctx.Matches(Labels.EndEvent))
        {
            var candidates = Teams.GetAll().Include(t => t.KmmTeam).Where(t => t.KmmTeam != null).ToList();
            if (candidates.Count == 0)
                return RetryWithMessage(ctx, message: Localization.NoTeamsForEvent);

            var state = new EventWinnersSelectionState(eventId, [.. candidates.Select(t => t.Id)], []);

            var keyboard = new List<List<KeyboardButton>>();
            foreach (var id in state.RemainingTeamIds)
            {
                var t = await Teams.GetByIdAsync(id);
                keyboard.Add([new(t!.Name)]);
            }

            await ctx.BotClient.SendMessage(ctx.Chat, Localization.PickTeamToProcess, replyMarkup: new ReplyKeyboardMarkup(keyboard) { ResizeKeyboard = true });
            // Start the flow by asking for the place of the first team
            return ToStateWith(nameof(OnSelectTeamToAskAsync), state);
        }

        return InvalidInput(ctx);
    }

    // First step: select which team to process next (text keyboard with remaining teams)
    // This state shows keyboard and transitions to PromptPlace state when a team is picked
    [MenuState(nameof(Localization.YouCanAlsoInputCSVWithResults))]
    [InheritKeyboard]
    public async Task<StateResult> OnSelectTeamToAskAsync(ModuleStateContext ctx)
    {
        if (!ctx.TryGetData(out EventWinnersSelectionState state)) return Fail();

        // If message text received - interpret as team pick
        if (ctx.Message is TextMessageContent msg)
        {
            var candidateTeams = Teams.GetAll().Include(t => t.KmmTeam).Where(t => state.RemainingTeamIds.Contains(t.Id)).ToList();
            var picked = candidateTeams.FirstOrDefault(t => t.Name == msg.Text);
            if (picked == null)
                return RetryWithMessage(ctx, message: Localization.InvalidTeamSelection);

            // Move picked team from Remaining to Processed (we will ask place then quest result)
            state.RemainingTeamIds.Remove(picked.Id);
            state.ProcessedTeamIds.Add(picked.Id);

            // Save intermediate state with the picked team id using record
            var pickHolder = new TeamSelectionHolder(state, picked.Id);
            return ToStateWith(nameof(OnPromptPlaceAsync), pickHolder);
        }
        else if (ctx.Message is FileMessageContent file)
        {
            using var stream = new MemoryStream();
            await ctx.BotClient.GetInfoAndDownloadFile(file.FileId, stream, ctx.CancellationToken);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            // Skip header
            string? line = await reader.ReadLineAsync();
            List<(Guid HackathonTeamId, KmmTeam Team, int Place, bool Completed)> teams = [];

            while ((line = await reader.ReadLineAsync()) != null)
            {
                try
                {
                    string[] parts = line.Split(';');
                    string teamName = parts[0];
                    int place = int.Parse(parts[1]);
                    bool completed = bool.Parse(parts[2]);

                    var team = await Teams.FindByNameAsync(teamName, ctx.CancellationToken);
                    if (team == null)
                        continue;

                    var kmmTeam = await KmmTeams.GetByIdAsync(team.KmmId);
                    if (kmmTeam == null)
                        continue;

                    teams.Add((team.Id, kmmTeam, place, completed));
                }
                catch
                {
                    return InvalidInput(ctx);
                }
            }

            List<Guid> processed = [];

            foreach (var rec in teams)
            {
                // Find EventEntry(ies) for this KmmTeam in this event
                var entries = await EventEntries.GetAll()
                    .Include(e => e.Quest)
                    .Where(e => e.KmmTeamId == rec.Team.Id && e.Quest.EventId == state.EventId)
                    .ToListAsync(ctx.CancellationToken);

                if (entries.Count > 0)
                {
                    foreach (var entry in entries)
                    {
                        entry.IsQuestCompleted = rec.Completed;
                        entry.Place = rec.Place;
                    }
                    await EventEntries.SaveChangesAsync(ctx.CancellationToken);
                }

                processed.Add(rec.HackathonTeamId);
            }

            await ProcessEventResultsFullAsync(state.EventId, processed, ctx.BotClient, ctx.CancellationToken);

            // Write audit EventEnded
            await Audit.AddAsync(new EventAuditEntry
            {
                EventType = EventType.EventEnded,
                InitiatorId = ctx.User.Id,
                LoggedAt = DateTime.UtcNow,
                Comment = state.EventId.ToString()
            }, ctx.CancellationToken);
            await Audit.SaveChangesAsync(ctx.CancellationToken);

            return Completed(Localization.EventEndedSuccessfully);
        }
        else
        {
            // Send keyboard with remaining teams
            var keyboard = new List<List<KeyboardButton>>();
            foreach (var id in state.RemainingTeamIds)
            {
                var t = await Teams.GetByIdAsync(id);
                keyboard.Add([new(t!.Name)]);
            }
            await ctx.BotClient.SendMessage(ctx.Chat, Localization.PickTeamToProcess, replyMarkup: new ReplyKeyboardMarkup(keyboard) { ResizeKeyboard = true });
            return ToStateWith(nameof(OnSelectTeamToAskAsync), state);
        }
    }

    // PromptState<int> to ask for place number; returns int input immediately
    [PromptState<int>(nameof(Localization.PromptPlace))]
    public Task<StateResult> OnPromptPlaceAsync(PromptStateContext<int> ctx)
    {
        // Expect the state to be a TeamSelectionHolder record
        if (!ctx.TryGetData(out TeamSelectionHolder holder))
            return Task.FromResult(Fail());

        if (!ctx.Input.TryGetValue(out int place))
            return Task.FromResult(InvalidInput(ctx));

        // store the place temporarily in a PlaceAssignmentHolder record and ask about quest completion
        var placeHolder = new PlaceAssignmentHolder(holder.State, holder.PickedTeamId, place);
        return Task.FromResult(ToStateWith(nameof(OnPromptQuestCompletedAsync), placeHolder));
    }

    // MenuState with Yes/No buttons to ask if quest was completed
    [MenuState(nameof(Localization.PromptQuestCompleted), BackButton = false)]
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public async Task<StateResult> OnPromptQuestCompletedAsync(ModuleStateContext ctx)
    {
        // Expect PlaceAssignmentHolder record
        if (!ctx.TryGetData(out PlaceAssignmentHolder holder))
            return Fail();

        bool? completed = null;
        if (ctx.Matches(Labels.Yes)) completed = true;
        else if (ctx.Matches(Labels.No)) completed = false;
        else return InvalidInput(ctx);

        // Persist results to EventEntry for this team's assigned quest(s)
        var team = await Teams.GetAll().Include(t => t.KmmTeam).FirstOrDefaultAsync(t => t.Id == holder.PickedTeamId, ctx.CancellationToken);
        var kmmTeam = team?.KmmTeam;
        if (kmmTeam == null)
        {
            // continue processing remaining or finish
            return await ContinueOrFinishAfterProcessing(holder.State, ctx);
        }

        // Find EventEntry(ies) for this KmmTeam in this event
        var entries = await EventEntries.GetAll()
            .Include(e => e.Quest)
            .Where(e => e.KmmTeamId == kmmTeam.Id && e.Quest.EventId == holder.State.EventId)
            .ToListAsync(ctx.CancellationToken);

        if (entries.Count > 0)
        {
            foreach (var entry in entries)
            {
                entry.IsQuestCompleted = completed;
                entry.Place = holder.Place;
            }
            await EventEntries.SaveChangesAsync(ctx.CancellationToken);
        }

        // Continue to next team or finalize
        return await ContinueOrFinishAfterProcessing(holder.State, ctx);
    }

    // Helper to continue or finish after one team processed
    private async Task<StateResult> ContinueOrFinishAfterProcessing(EventWinnersSelectionState state, ModuleStateContext ctx)
    {
        // If there are remaining teams, go back to team selection
        if (state.RemainingTeamIds.Count > 0)
        {
            // Send keyboard for next selection
            var keyboard = new List<List<KeyboardButton>>();
            foreach (var id in state.RemainingTeamIds)
            {
                var t = await Teams.GetByIdAsync(id);
                keyboard.Add([new(t!.Name)]);
            }
            await ctx.BotClient.SendMessage(ctx.Chat, Localization.PickTeamToProcess, replyMarkup: new ReplyKeyboardMarkup(keyboard) { ResizeKeyboard = true });
            return ToStateWith(nameof(OnSelectTeamToAskAsync), state);
        }

        // All teams processed — finalize event
        await ProcessEventResultsFullAsync(state.EventId, state.ProcessedTeamIds, ctx.BotClient, ctx.CancellationToken);

        // Write audit EventEnded
        await Audit.AddAsync(new EventAuditEntry
        {
            EventType = EventType.EventEnded,
            InitiatorId = ctx.User.Id,
            LoggedAt = DateTime.UtcNow,
            Comment = state.EventId.ToString()
        }, ctx.CancellationToken);
        await Audit.SaveChangesAsync(ctx.CancellationToken);

        return Completed(Localization.EventEndedSuccessfully);
    }

    // Full processing across all participating teams using saved EventEntry values (place and IsQuestCompleted)
    private async Task ProcessEventResultsFullAsync(long eventId, List<Guid> processedTeamIds, ITelegramBotClient botClient, CancellationToken ct)
    {
        // Load audit times for last event started/ended bounding the event's actions
        var lastEventStarted = (from ev in await Audit.GetByTypeAsync(EventType.EventStarted, ct)
                                orderby ev.LoggedAt descending
                                select ev.LoggedAt).FirstOrDefault();
        var lastEventEnded = (from ev in await Audit.GetByTypeAsync(EventType.EventEnded, ct)
                              orderby ev.LoggedAt descending
                              select ev.LoggedAt).FirstOrDefault();

        var currentEvent = await Events.GetByIdWithQuestsAsync(eventId, ct) ?? throw new InvalidOperationException();
        int scoreToAdd = 0, totalScoreToAdd = 0;

        List<Team> teams = [];

        foreach (var id in processedTeamIds)
        {
            var team = await Teams.GetAll()
                .Include(t => t.KmmTeam)
                .ThenInclude(k => k!.AbilitiesLog)
                .FirstOrDefaultAsync(t => t.Id == id, ct);
            if (team == null)
                continue;
            teams.Add(team);
        }

        KmmTeam? godfatherTeam = teams.FirstOrDefault(t => t.KmmTeam?.Role == MafiaRole.Godfather)?.KmmTeam;

        foreach (var team in teams.Where(x => x.KmmTeam != null).OrderBy(x => x.KmmTeam!.Role))
        {
            var kmm = team.KmmTeam!;

            // Load event entries for this KmmTeam connected to the event
            var entry = await EventEntries.GetAll()
                .Where(e => e.KmmTeamId == kmm.Id && e.Quest.EventId == eventId)
                .FirstOrDefaultAsync(ct);

            if (entry == null)
                continue;

            // Use entry.Place and entry.IsQuestCompleted for post-processing
            int place = entry.Place;
            bool completed = entry.IsQuestCompleted ?? false;

            int reward = place switch
            {
                1 => currentEvent.FirstPlaceReward,
                2 => currentEvent.SecondPlaceReward,
                3 => currentEvent.ThirdPlaceReward,
                _ => 0,
            };
            int coins = reward;
            if (!kmm.IsAlive)
                coins = 0;

            if (completed)
            {
                if (kmm.Role == MafiaRole.Mafia || kmm.Role == MafiaRole.Godfather)
                {
                    if (kmm.IsAlive)
                    {
                        int stolen = (int)(totalScoreToAdd * _config.StealSuccessCoefficient);
                        coins += stolen;
                        scoreToAdd -= stolen;

                        // Add bonus to godfather (if this is not godfather)
                        if (godfatherTeam != null && kmm != godfatherTeam)
                        {
                            int godfatherBank = await Banks.GetCoinsAsync(godfatherTeam.Color, ct);
                            await Banks.SetCoinsAsync(godfatherTeam.Color, godfatherBank + (int)(stolen * _config.GodfatherCoefficient), ct);
                        }

                        int mafiaBank = await Banks.GetCoinsAsync(kmm.Color, ct);
                        await Banks.SetCoinsAsync(kmm.Color, mafiaBank + coins, ct);
                    }
                }
                else
                {
                    reward = (int)(reward * _config.QuestRewardCoefficient);
                    if (kmm.AbilitiesLog.Where(x => x.UsedAt >= lastEventEnded).Any(x => x.Ability == Ability.WorkEthic))
                        coins = (int)(coins * _config.CitizenSkillCoefficient);

                    scoreToAdd += coins;
                    totalScoreToAdd += coins;
                }
            }

            kmm.Score += reward;

            // Any additional per-team finalization (e.g., notifications)
            var members = team!.Members;
            var text = Localization.EventFinalResultForTeam.FormatInvariant(eventId, kmm.Color, place, completed.AsEmoji().ToUnicode(), kmm.Score);
            foreach (var m in members)
            {
                if (m.TelegramId == 0 || m.TelegramId is not long tg) continue;
                try
                {
                    await botClient.SendMessage(new ChatId(tg), text, cancellationToken: ct);
                }
                catch { }
            }
        }
        await KmmTeams.SaveChangesAsync(ct);

        int citizensBank = await Banks.GetParticipantCoinsAsync(ct);
        await Banks.SetParticipantCoinsAsync(citizensBank + scoreToAdd, ct);
    }

    [MenuState(nameof(Localization.SelectQuest))]
    [InheritKeyboard]
    public async Task<StateResult> OnSelectQuestToManageAsync(ModuleStateContext ctx)
    {
        if (ctx.Message is not TextMessageContent msg)
            return InvalidInput(ctx);

        if (!ctx.TryGetData(out long eventId))
            return Fail();

        var quest = await Quests.FindByNameAsync(eventId, msg.Text, ctx.CancellationToken);
        if (quest == null)
            return InvalidInput(ctx);

        return ToStateWith(nameof(OnQuestManageMenuAsync), new EventQuest(eventId, quest.Id));
    }

    private readonly record struct EventQuest(long EventId, long QuestId);

    [MenuState(nameof(Localization.ModuleSelectRootMenu), ParentStateName = nameof(OnManageEventAsync))]
    [MenuItem(nameof(Labels.EditQuestName))]
    [MenuItem(nameof(Labels.EditQuestDescription))]
    [MenuItem(nameof(Labels.DeleteQuest))]
    public async Task<StateResult> OnQuestManageMenuAsync(ModuleStateContext ctx)
    {
        if (!ctx.TryGetData(out EventQuest data))
            return Fail();

        var quest = await Quests.GetByIdAsync(data.QuestId);
        if (quest == null)
            return Fail();

        if (ctx.Matches(Labels.EditQuestName))
        {
            return ToStateWith(nameof(OnPromptEditQuestNameAsync), data);
        }
        else if (ctx.Matches(Labels.EditQuestDescription))
        {
            return ToStateWith(nameof(OnPromptEditQuestDescriptionAsync), data);
        }
        else if (ctx.Matches(Labels.DeleteQuest))
        {
            return ToStateWith(nameof(OnConfirmDeleteQuestAsync), data);
        }

        return InvalidInput(ctx);
    }

    [PromptState<string>(nameof(Localization.InputNewQuestName))]
    public async Task<StateResult> OnPromptEditQuestNameAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.Input.TryGetValue(out string newName))
            return InvalidInput(ctx);

        if (!ctx.TryGetData(out EventQuest data))
            return Fail();

        var quest = await Quests.GetByIdAsync(data.QuestId);
        if (quest == null) return Fail();

        if (await Quests.FindByNameAsync(quest.EventId, newName, ctx.CancellationToken) != null)
            return RetryWith(ctx, data, message: Localization.NameTaken);

        quest.Name = newName;
        await Quests.SaveChangesAsync(ctx.CancellationToken);
        return Completed(Localization.QuestUpdated);
    }

    [PromptState<string>(nameof(Localization.InputNewQuestDescription))]
    public async Task<StateResult> OnPromptEditQuestDescriptionAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.Input.TryGetValue(out string newDescription))
            return InvalidInput(ctx);

        if (!ctx.TryGetData(out EventQuest q))
            return Fail();

        var quest = await Quests.GetByIdAsync(q.QuestId);
        if (quest == null) return Fail();

        quest.Description = newDescription;
        await Quests.SaveChangesAsync(ctx.CancellationToken);
        return Completed(Localization.QuestUpdated);
    }

    [MenuState(nameof(Localization.ConfirmQuestDeletion))]
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public async Task<StateResult> OnConfirmDeleteQuestAsync(ModuleStateContext ctx)
    {
        if (!ctx.TryGetData(out EventQuest q))
            return Fail();

        if (ctx.Matches(Labels.Yes))
        {
            var quest = await Quests.GetByIdAsync(q.QuestId);
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
        if (!ctx.TryGetData(out long eventId))
            return Fail();

        if (!ctx.Input.TryGetValue(out string name))
            return InvalidInput(ctx);
        var quest = await Quests.FindByNameAsync(eventId, name, ctx.CancellationToken);
        if (quest != null)
            return InvalidInput(ctx);
        return ToStateWith(nameof(OnInputQuestDescriptionAsync), new QuestInputModel(eventId, name, string.Empty));
    }

    private readonly record struct QuestInputModel(long EventId, string Name, string Description);

    [PromptState<string>(nameof(Localization.InputQuestDescription))]
    public Task<StateResult> OnInputQuestDescriptionAsync(PromptStateContext<string> ctx)
    {
        if (!ctx.TryGetData(out QuestInputModel data))
            return Task.FromResult(Fail());
        if (!ctx.Input.TryGetValue(out string description))
            return Task.FromResult(InvalidInput(ctx));
        return Task.FromResult(ToStateWith(nameof(OnSelectQuestIsSabotageAsync), data with { Description = description }));
    }

    [MenuState(nameof(Localization.SelectQuestIsSabotage))]
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public async Task<StateResult> OnSelectQuestIsSabotageAsync(ModuleStateContext ctx)
    {
        bool isSabotage;
        if (ctx.Matches(Labels.Yes)) isSabotage = true;
        else if (ctx.Matches(Labels.No)) isSabotage = false;
        else return InvalidInput(ctx);

        if (!ctx.TryGetData(out QuestInputModel data))
            return Fail();

        var quest = await Quests.FindByNameAsync(data.EventId, data.Name, ctx.CancellationToken);
        if (quest != null)
            return Fail();

        quest = new()
        {
            EventId = data.EventId,
            Name = data.Name,
            Description = data.Description,
            IsSabotage = isSabotage,
        };
        await Quests.AddAsync(quest, ctx.CancellationToken);
        await Quests.SaveChangesAsync(ctx.CancellationToken);
        return ToStateWith(nameof(OnManageEventAsync), data.EventId, message: Localization.QuestCreatedSuccessfully);
    }

    [MenuState(nameof(Localization.ConfirmEventDeletion))]
    [MenuRow(nameof(Labels.No), nameof(Labels.Yes))]
    public async Task<StateResult> OnConfirmDeleteEventAsync(ModuleStateContext ctx)
    {
        if (ctx.Matches(Labels.Yes))
        {
            if (!ctx.TryGetData(out long eventId))
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

    public readonly record struct EventInputModel(
        [property: Display(Prompt = nameof(Localization.InputEventName))]
        [property: Required(ErrorMessage = nameof(Localization.BaseModuleInvalidInput))]
        [property: CheckEventName]
        string Name,
        [property: Display(Prompt = nameof(Localization.InputFirstPlaceReward))]
        int FirstPlace,
        [property: Display(Prompt = nameof(Localization.InputSecondPlaceReward))]
        int SecondPlace,
        [property: Display(Prompt = nameof(Localization.InputThirdPlaceReward))]
        int ThirdPlace);

    [ModelPromptState<EventInputModel>]
    public async Task<StateResult> OnInputEventModelAsync(ModelPromptContext<EventInputModel> ctx)
    {
        var ev = await Events.FindByNameAsync(ctx.Model.Name, ctx.CancellationToken);
        if (ev != null)
            return FailWithMessage(Localization.NameTaken);

        ev = new()
        {
            Name = ctx.Model.Name,
            FirstPlaceReward = ctx.Model.FirstPlace,
            SecondPlaceReward = ctx.Model.SecondPlace,
            ThirdPlaceReward = ctx.Model.ThirdPlace,
        };

        await Events.AddAsync(ev, ctx.CancellationToken);
        await Events.SaveChangesAsync(ctx.CancellationToken);
        return Completed(Localization.EventCreated);
    }
}