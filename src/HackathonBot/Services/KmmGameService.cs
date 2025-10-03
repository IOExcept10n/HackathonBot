using HackathonBot.Models;
using HackathonBot.Models.Kmm;
using HackathonBot.Modules;
using HackathonBot.Repository;
using Microsoft.EntityFrameworkCore;

namespace HackathonBot.Services
{
    public class KmmGameService(
        IKmmTeamRepository teamRepo,
        IAbilityUseRepository abilityRepo,
        IEventAuditRepository auditRepo,
        IEventRepository eventRepo,
        IEventEntryRepository eventEntryRepo,
        IBankRepository bankRepo) : IKmmGameService
    {
        private readonly IKmmTeamRepository _teamRepo = teamRepo;
        private readonly IAbilityUseRepository _abilityRepo = abilityRepo;
        private readonly IEventAuditRepository _auditRepo = auditRepo;
        private readonly IEventRepository _eventRepo = eventRepo;
        private readonly IEventEntryRepository _eventEntryRepo = eventEntryRepo;
        private readonly IBankRepository _bankRepo = bankRepo;
        private readonly Random _rng = new();

        public async Task InitializeGameAsync(IEnumerable<Team> participatingTeams, KmmConfig options, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(participatingTeams);
            ArgumentNullException.ThrowIfNull(options);

            // 1. Clear tables: AbilityUse, Bank, EventAuditEntry, EventEntry, KmmTeam
            // Use TruncateAsync if available (preferred). Otherwise enumerate & delete.
            await _abilityRepo.TruncateAsync(ct);
            await _bankRepo.TruncateAsync(ct);
            await _auditRepo.TruncateAsync(ct);
            await _eventEntryRepo.TruncateAsync(ct);
            await _teamRepo.TruncateAsync(ct);

            // Note: keep Quest and Event as requested (do not truncate _eventRepo/_questRepo).

            // 2. Prepare list of teams
            var teamsList = participatingTeams.ToList();
            if (teamsList.Count == 0)
                return;

            // 3. Determine counts
            var total = teamsList.Count;
            var mafiaCount = Math.Clamp(options.MafiaLimit, 0, total); // ensure bounds

            // Ensure mafiaCount includes Godfather if godfather is considered mafia (we will treat Godfather as mafia role).
            // Adjust non-overlap: Godfather is one of mafia slots. So ensure mafiaCount >= 1 if total allows and godfather must exist.
            if (total >= 3) // need at least 3 players to have required roles
            {
                if (mafiaCount < 1) mafiaCount = 1;
            }

            // 4. Build roles pool
            var rng = _rng;

            var indices = Enumerable.Range(0, total).ToList();
            // shuffle indices
            for (int i = indices.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (indices[j], indices[i]) = (indices[i], indices[j]);
            }

            var roles = new MafiaRole[total];

            // assign Godfather (ensure exactly one)
            roles[indices[0]] = MafiaRole.Godfather;

            // assign detective and doctor (ensure unique and not overlapping with godfather)
            if (total >= 3)
            {
                roles[indices[1]] = MafiaRole.Detective;
                roles[indices[2]] = MafiaRole.Doctor;
            }
            else
            {
                // fallback: if less than 3 teams, fill remaining with citizens
                for (int k = 1; k < total; k++) roles[indices[k]] = MafiaRole.Citizen;
            }

            // assign remaining mafia (mafiaCount includes Godfather). Already assigned 1 godfather.
            int remainingMafiaToAssign = Math.Max(0, mafiaCount - 1);
            int pos = 3;
            while (remainingMafiaToAssign > 0 && pos < total)
            {
                roles[indices[pos]] = MafiaRole.Mafia;
                remainingMafiaToAssign--;
                pos++;
            }

            // fill any unassigned with Citizen
            for (int i = 0; i < total; i++)
                if (roles[i] == 0) // default enum value is Citizen (0) — but to be explicit:
                    roles[i] = MafiaRole.Citizen;

            // 5. Create KmmTeam entries and optionally initialize bank balances
            // We'll create KmmTeam for each Team, set Role, Score default 0, IsAlive = true, HackathonTeamId = Team.Id
            foreach (var pair in teamsList.Select((t, idx) => (team: t, idx)))
            {
                var team = pair.team;
                var idx = pair.idx;

                team.KmmId = null;

                var kmm = new KmmTeam
                {
                    HackathonTeamId = team.Id,
                    Role = roles[idx],
                    Score = 0,
                    IsAlive = true
                };

                await _teamRepo.AddAsync(kmm, ct);

                // If repository is not tracking and AddAsync does not propagate DB-generated Id to kmm,
                // we rely on SaveChanges below. But we must set Team.KmmId to created id.
                // If tracking context is used, the Id will be set after SaveChanges. To be safe, call SaveChanges after adding all.
                team.KmmTeam = kmm;
            }

            // 6. Persist changes. If repositories use tracking context, SaveChangesAsync may be no-op but call anyway.
            await _teamRepo.SaveChangesAsync(ct);

            foreach (var team in teamsList)
                team.KmmId = team.KmmTeam!.Id;

            await _abilityRepo.SaveChangesAsync(ct);
            await _bankRepo.SaveChangesAsync(ct);
            await _auditRepo.SaveChangesAsync(ct);
            await _eventEntryRepo.SaveChangesAsync(ct);
        }

        public async Task<DayResult> SimulateDayAsync(CancellationToken ct = default)
        {
            // 1. Get last VoteStarted and VoteEnded audit entries
            var voteStarted = (await _auditRepo.GetByTypeAsync(EventType.VoteStarted, ct)).OrderByDescending(a => a.LoggedAt).FirstOrDefault();
            var voteEnded = (await _auditRepo.GetByTypeAsync(EventType.VoteEnded, ct)).OrderByDescending(a => a.LoggedAt).FirstOrDefault();

            if (voteStarted == null)
                throw new InvalidOperationException("No voting audit entries found. Start voting is required.");

            if (voteEnded != null && voteStarted.LoggedAt < voteEnded.LoggedAt)
                throw new InvalidOperationException("VoteStarted is older than VoteEnded. You must start voting before ending the day simulation.");

            // Per your rule: use abilities with UsedAt > last VoteEnded.LoggedAt
            var lastVoteEndedTime = voteEnded?.LoggedAt ?? DateTime.UtcNow;
            var lastVoteStartedTime = voteStarted.LoggedAt;

            // Get all alive teams
            var aliveTeams = (await _teamRepo.GetAliveTeamsAsync(ct)).Where(t => t.IsAlive).ToList();

            // Get all ability uses after lastVoteEndedTime
            var allAbilities = _abilityRepo.GetAll().Where(a => a.UsedAt > lastVoteEndedTime).ToList();

            // Filter abilities to only those by currently alive teams and one-per-team-per-ability enforced by external service assumption
            allAbilities = [.. allAbilities.Where(a => aliveTeams.Any(t => t.Id == a.TeamId))];

            // Determine global flags: Firewall used? HackerAttack_Defense used? HackerAttack_Check used? Slander targets etc.
            bool firewallUsed = allAbilities.Any(a => a.Ability == Ability.Firewall);
            bool hackerDefenseUsed = allAbilities.Any(a => a.Ability == Ability.HackerAttack && a.TargetTeamId == null && a.Team != null); // treated as global
            bool hackerCheckUsed = allAbilities.Any(a => a.Ability == Ability.HackerAttack && a.TargetTeamId == null && a.Team != null); // same global marker (kept for clarity)

            // More precise: you specified HackerAttack_Defense and HackerAttack_Check as separate abilities,
            // but in enum we have only HackerAttack. Assume HackerAttack can block both unless Firewall used.
            bool hackerAttackPresent = allAbilities.Any(a => a.Ability == Ability.HackerAttack);
            bool hackerBlocks = hackerAttackPresent && !firewallUsed;

            // Collect per-team abilities
            var abilitiesByTeam = allAbilities.OrderByDescending(a => a.UsedAt).GroupBy(a => a.TeamId).ToDictionary(g => g.Key, g => g.ToList());

            // 2. Compute citizens (including detective & doctor) votes first.
            var citizenVotes = new Dictionary<long, double>(); // targetTeamId -> weighted votes
            var mafiaVotes = new Dictionary<long, double>();

            // Slander: if applied to some target by Godfather, that target loses their vote
            var slanderTargets = new HashSet<long>(allAbilities.Where(a => a.Ability == Ability.Slander && a.TargetTeamId.HasValue).Select(a => a.TargetTeamId!.Value));

            // Firewall already accounted for; HackerAttack if present blocks Check/Defense
            bool checkBlocked = hackerBlocks;
            bool defenseBlocked = hackerBlocks;

            // Count votes: iterate alive teams, skip those whose vote is removed by Slander
            foreach (var team in aliveTeams)
            {
                // If team's vote stripped by Slander, skip
                if (slanderTargets.Contains(team.Id))
                    continue;

                // Find Vote ability use by this team in current round (if any)
                var teamAbilities = abilitiesByTeam.TryGetValue(team.Id, out var list) ? list : [];
                var voteUse = teamAbilities.FirstOrDefault(a => a.Ability == Ability.Vote && a.TargetTeamId.HasValue && a.UsedAt > lastVoteEndedTime && a.UsedAt >= lastVoteStartedTime);

                if (voteUse == null)
                    continue; // no vote cast

                var targetId = voteUse.TargetTeamId!.Value;
                var weight = 1.0;

                if (team.Role == MafiaRole.Detective || team.Role == MafiaRole.Godfather)
                    weight = 1.1;

                // Determine block by role: if this team is mafia or godfather -> mafia block, else citizens block
                if (team.Role == MafiaRole.Mafia || team.Role == MafiaRole.Godfather)
                {
                    mafiaVotes[targetId] = mafiaVotes.GetValueOrDefault(targetId) + weight;
                }
                else
                {
                    citizenVotes[targetId] = citizenVotes.GetValueOrDefault(targetId) + weight;
                }
            }

            // 3. Determine lynched by citizens (vote by citizens happens first). Citizens exclude one team guaranteed.
            KmmTeam? lynched = null;
            if (citizenVotes.Count == 0)
            {
                // If nobody voted among citizens, pick random alive non-mafia team (or any alive)
                var potential = aliveTeams.Where(t => t.Role != MafiaRole.Mafia && t.Role != MafiaRole.Godfather).ToList();
                if (potential.Count == 0)
                    potential = aliveTeams;
                lynched = potential[_rng.Next(potential.Count)];
            }
            else
            {
                // pick target with max votes; if tie, random among equals
                var max = citizenVotes.Max(kv => kv.Value);
                var tied = citizenVotes.Where(kv => Math.Abs(kv.Value - max) < 1e-6).Select(kv => kv.Key).ToList();
                var chosenId = tied.Count == 1 ? tied[0] : tied[_rng.Next(tied.Count)];
                lynched = aliveTeams.FirstOrDefault(t => t.Id == chosenId);
            }

            // Mark lynched as dead for remainder of day (their abilities after lynch are not counted)
            if (lynched != null)
            {
                lynched.IsAlive = false;
            }

            // 4. Doctor applies Defense (if not blocked by HackerAttack). Doctor target can protect one team.
            KmmTeam? doctorProtectTarget = null;
            var doctorTeam = aliveTeams.FirstOrDefault(t => t.Role == MafiaRole.Doctor && t.IsAlive);
            if (doctorTeam != null && !defenseBlocked)
            {
                var doctorAbilities = abilitiesByTeam.TryGetValue(doctorTeam.Id, out var dlist) ? dlist : new List<AbilityUse>();
                var defenseUse = doctorAbilities.FirstOrDefault(a => (a.Ability == Ability.Defense || a.Ability == Ability.Firewall) && a.TargetTeamId.HasValue && a.UsedAt > lastVoteEndedTime);
                if (defenseUse != null)
                    doctorProtectTarget = aliveTeams.FirstOrDefault(t => t.Id == defenseUse.TargetTeamId!.Value);
            }

            // 5. Detective applies Check (if not blocked). The check result can be cancelled (null) if blocked.
            KmmTeam? detectiveTarget = null;
            bool? detectiveResult = null;
            var detectiveTeam = aliveTeams.FirstOrDefault(t => t.Role == MafiaRole.Detective && t.IsAlive);
            if (detectiveTeam != null && !checkBlocked)
            {
                var detAbilities = abilitiesByTeam.TryGetValue(detectiveTeam.Id, out var listDet) ? listDet : [];
                var checkUse = detAbilities.FirstOrDefault(a => a.Ability == Ability.Check && a.TargetTeamId.HasValue && a.UsedAt > lastVoteEndedTime);
                if (checkUse != null)
                {
                    detectiveTarget = aliveTeams.FirstOrDefault(t => t.Id == checkUse.TargetTeamId!.Value);
                    if (detectiveTarget != null)
                        detectiveResult = detectiveTarget.Role == MafiaRole.Mafia || detectiveTarget.Role == MafiaRole.Godfather;
                }
            }
            else if (detectiveTeam != null && checkBlocked)
            {
                // blocked -> null result, attempt to find target for reporting
                var detAbilities = abilitiesByTeam.TryGetValue(detectiveTeam.Id, out var listDet) ? listDet : [];
                var checkUse = detAbilities.FirstOrDefault(a => a.Ability == Ability.Check && a.TargetTeamId.HasValue && a.UsedAt > lastVoteEndedTime);
                if (checkUse != null)
                    detectiveTarget = aliveTeams.FirstOrDefault(t => t.Id == checkUse.TargetTeamId!.Value);
                detectiveResult = null;
            }

            // 6. Mafia votes (including Godfather). Note: abilities of lynched team should not be counted — we already marked lynched.IsAlive=false.
            // Recompute mafia votes but skip votes from lynched team if it was mafia.
            var mafiaVotesFiltered = new Dictionary<long, double>();
            foreach (var kv in mafiaVotes)
            {
                mafiaVotesFiltered[kv.Key] = kv.Value;
            }

            // If lynched had voted, remove their contribution (we earlier included them). Ensure we remove: recalc from abilitiesByTeam excluding lynched.
            if (lynched != null)
            {
                mafiaVotesFiltered.Clear();
                foreach (var team in aliveTeams.Where(t => t.IsAlive && (t.Role == MafiaRole.Mafia || t.Role == MafiaRole.Godfather)))
                {
                    var teamAbilities = abilitiesByTeam.TryGetValue(team.Id, out var list) ? list : [];
                    var voteUse = teamAbilities.FirstOrDefault(a => a.Ability == Ability.Vote && a.TargetTeamId.HasValue && a.UsedAt > lastVoteEndedTime && a.UsedAt >= lastVoteStartedTime);
                    if (voteUse == null) continue;
                    var targetId = voteUse.TargetTeamId!.Value;
                    var weight = (team.Role == MafiaRole.Detective || team.Role == MafiaRole.Godfather) ? 1.1 : 1.0; // detective won't be mafia, but keep rule
                    mafiaVotesFiltered[targetId] = mafiaVotesFiltered.GetValueOrDefault(targetId) + weight;
                }
            }

            // Determine mafia kill target: pick max from mafiaVotesFiltered; if none, random alive non-mafia?
            KmmTeam? killedByMafia = null;
            if (mafiaVotesFiltered.Count == 0)
            {
                // choose random non-mafia alive (excluding lynched)
                var potential = aliveTeams.Where(t => t.IsAlive && t.Role != MafiaRole.Mafia && t.Role != MafiaRole.Godfather).ToList();
                if (potential.Count == 0)
                    potential = [.. aliveTeams.Where(t => t.IsAlive)];
                if (potential.Count != 0)
                    killedByMafia = potential[_rng.Next(potential.Count)];
            }
            else
            {
                var max = mafiaVotesFiltered.Max(kv => kv.Value);
                var tied = mafiaVotesFiltered.Where(kv => Math.Abs(kv.Value - max) < 1e-6).Select(kv => kv.Key).ToList();
                var chosenId = tied.Count == 1 ? tied[0] : tied[_rng.Next(tied.Count)];
                killedByMafia = aliveTeams.FirstOrDefault(t => t.Id == chosenId && t.IsAlive);
            }

            // 7. Apply doctor's protection: if doctor protected the killedByMafia target, they survive.
            if (killedByMafia != null && doctorProtectTarget != null && killedByMafia.Id == doctorProtectTarget.Id)
            {
                // protected -> not killed
                killedByMafia = null;
            }
            else if (killedByMafia != null)
            {
                // kill the target
                killedByMafia.IsAlive = false;
            }

            // 8. Prepare DayResult with detective check result and targets; lynched is already set (may be mafia or citizen)
            var dayResult = new DayResult
            {
                KilledByMafia = killedByMafia,
                LynchedByCitizens = lynched,
                DetectiveCheckResult = detectiveTarget == null ? null : new(detectiveTarget, detectiveResult)
            };

            // 9. Persist changes: update team alive statuses and save ability/event changes if needed.
            await _teamRepo.SaveChangesAsync(ct);

            return dayResult;
        }

        public async Task<GameStats> GetGameStatsAsync(CancellationToken ct = default)
        {
            var alive = (await _teamRepo.GetAliveTeamsAsync(ct)).Where(t => t.IsAlive).ToList();
            var aliveMafia = alive.Count(t => t.Role == MafiaRole.Mafia || t.Role == MafiaRole.Godfather);
            var alivePeaceful = alive.Count - aliveMafia;
            GameStats.MafiaSide? winner = null;
            if (aliveMafia == 0) winner = GameStats.MafiaSide.Citizens;
            else if (aliveMafia >= alivePeaceful) winner = GameStats.MafiaSide.Mafia;
            else winner = GameStats.MafiaSide.None;

            return new GameStats
            {
                AliveMafia = aliveMafia,
                AlivePeaceful = alivePeaceful,
                Winner = winner
            };
        }

        public async Task<bool> CanUseAbilityAsync(KmmTeam team, Ability ability, CancellationToken ct = default)
        {
            // Check last use for this day — ability usage regulation assumed external; here we check that team did not use same ability after last VoteEnded
            var voteEnded = (await _auditRepo.GetByTypeAsync(EventType.VoteEnded, ct)).OrderByDescending(a => a.LoggedAt).FirstOrDefault();
            var lastVoteEnded = voteEnded?.LoggedAt ?? DateTime.MinValue;
            var lastUse = (await _abilityRepo.GetLastUseAsync(team.Id, ability, ct));
            if (lastUse == null) return true;
            return lastUse.UsedAt <= lastVoteEnded;
        }

        public async Task<KmmTeam?> GetKmmTeamByTelegramIdAsync(long telegramId, CancellationToken ct = default)
        {
            // Assuming Team has Participants with TelegramId
            var all = await _teamRepo.GetAll().Include(x => x.HackathonTeam).ThenInclude(x => x.Members).ToListAsync(ct);
            foreach (var t in all)
            {
                if (t.HackathonTeam.Members.Any(p => p.TelegramId == telegramId) == true)
                    return t;
            }
            return null;
        }

        public async Task<IEnumerable<VoteDto>> GetTodayVotesAsync(CancellationToken ct = default)
        {
            var lastVoteStarted = (await _auditRepo.GetByTypeAsync(EventType.VoteStarted, ct))
                .OrderByDescending(a => a.LoggedAt)
                .FirstOrDefault()?.LoggedAt ?? DateTime.MinValue;

            var abilities = await GetTodayAbilityUsesAsync(ct);

            var alive = (await _teamRepo.GetAliveTeamsAsync(ct)).Where(t => t.IsAlive).ToList();

            var res = abilities
                .Where(a => a.Ability == Ability.Vote && a.Team.IsAlive && a.TargetTeamId.HasValue && a.UsedAt > lastVoteStarted)
                .GroupBy(a => a.Team.Id)
                .Select(g =>
                    g.OrderByDescending(a => a.UsedAt)
                     .First())
                .Select(a => new VoteDto(a.Team, a.TargetTeam!))
                .ToList();

            return res;
        }

        public async Task<IEnumerable<AbilityUse>> GetTodayAbilityUsesAsync(CancellationToken ct = default)
        {
            var voteEnded = (await _auditRepo.GetByTypeAsync(EventType.VoteEnded, ct)).OrderByDescending(a => a.LoggedAt).FirstOrDefault();
            var lastVoteEnded = voteEnded?.LoggedAt ?? DateTime.MinValue;
            
            var abilities = _abilityRepo.GetAll().AsNoTracking()
                                .Include(a => a.Team)
                                .Include(a => a.TargetTeam)
                                .Where(a => a.UsedAt > lastVoteEnded && a.Team.IsAlive).ToList();

            return abilities;
        }
    }
}
