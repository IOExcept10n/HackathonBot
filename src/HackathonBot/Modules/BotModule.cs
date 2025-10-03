using HackathonBot.Repository;
using HackathonBot.Services;
using Microsoft.Extensions.DependencyInjection;
using MyBots.Core.Persistence.Repository;
using MyBots.Modules.Common;
using MyBots.Modules.Common.Interactivity;
using MyBots.Modules.Common.Roles;

namespace HackathonBot.Modules;

internal abstract class BotModule(ButtonLabel label, IEnumerable<Role> allowedRoles, IServiceProvider services)
    : ModuleBase(label, allowedRoles, services)
{
    protected readonly Lazy<IBankRepository> _bankRepository = CreateLazy<IBankRepository>(services);
    protected readonly Lazy<IKmmTeamRepository> _kmmTeamRepository = CreateLazy<IKmmTeamRepository>(services);
    protected readonly Lazy<IAbilityUseRepository> _abilityUseRepository = CreateLazy<IAbilityUseRepository>(services);
    protected readonly Lazy<IQuestRepository> _questRepository = CreateLazy<IQuestRepository>(services);
    protected readonly Lazy<IEventRepository> _eventRepository = CreateLazy<IEventRepository>(services);
    protected readonly Lazy<IEventEntryRepository> _eventEntryRepository = CreateLazy<IEventEntryRepository>(services);
    protected readonly Lazy<IEventAuditRepository> _auditRepository = CreateLazy<IEventAuditRepository>(services);

    protected readonly Lazy<IUserRepository> _FsmUserRepository = CreateLazy<IUserRepository>(services);
    protected readonly Lazy<IParticipantRepository> _iParticipantRepository = CreateLazy<IParticipantRepository>(services);
    protected readonly Lazy<ITeamRepository> _iTeamRepository = CreateLazy<ITeamRepository>(services);
    protected readonly Lazy<ISubmissionRepository> _iSubmissionRepository = CreateLazy<ISubmissionRepository>(services);
    protected readonly Lazy<IBotUserRoleRepository> _iBotUserRoleRepository = CreateLazy<IBotUserRoleRepository>(services);

    protected readonly Lazy<ITelegramUserService> _userService = CreateLazy<ITelegramUserService>(services);

    protected IEventAuditRepository Audit => _auditRepository.Value;
    protected IBankRepository Banks => _bankRepository.Value;
    protected IKmmTeamRepository KmmTeams => _kmmTeamRepository.Value;
    protected IAbilityUseRepository AbilityUses => _abilityUseRepository.Value;
    protected IQuestRepository Quests => _questRepository.Value;
    protected IEventRepository Events => _eventRepository.Value;
    protected IEventEntryRepository EventEntries => _eventEntryRepository.Value;
    protected IUserRepository FsmUsers => _FsmUserRepository.Value;
    protected IParticipantRepository Participants => _iParticipantRepository.Value;
    protected ITeamRepository Teams => _iTeamRepository.Value;
    protected ISubmissionRepository Submissions => _iSubmissionRepository.Value;
    protected IBotUserRoleRepository BotRoles => _iBotUserRoleRepository.Value;
    
    protected ITelegramUserService Users => _userService.Value;

    private static Lazy<T> CreateLazy<T>(IServiceProvider services) where T : class => new(services.GetRequiredService<T>);
}
