using System.ComponentModel.DataAnnotations;
using System.Reflection;
using HackathonBot.Models.Kmm;
using HackathonBot.Properties;

namespace HackathonBot.Repository.Validation
{
    // Common attribute for checking name uniqueness
    public abstract class UniqueNameAttribute(string? errorMessage = null) : ValidationAttribute(errorMessage ?? Localization.NameTaken)
    {
        protected abstract Task<bool> ExistsAsync(object? value, ValidationContext context, CancellationToken ct);

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // null/empty — don't validate here (you can use Required)
            var s = value as string;
            if (string.IsNullOrWhiteSpace(s))
                return ValidationResult.Success;

            try
            {
                var existsTask = ExistsAsync(value, validationContext, CancellationToken.None);
                var exists = Task.Run(() => existsTask).Result; // sync wait
                if (exists)
                    return new ValidationResult(ErrorMessage ?? Localization.NameTaken, [ validationContext.MemberName ?? string.Empty ]);
                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                // If got an error, return ValidationResult with message
                return new ValidationResult($"Validation failed: {ex.Message}");
            }
        }
    }

    // ITeamRepository.FindByNameAsync
    public class CheckTeamNameAttribute : UniqueNameAttribute
    {
        public CheckTeamNameAttribute() : base(Localization.NameTaken) { }

        protected override async Task<bool> ExistsAsync(object? value, ValidationContext context, CancellationToken ct)
        {
            var name = value as string;
            if (string.IsNullOrWhiteSpace(name)) return false;

            var repo = context.GetService(typeof(ITeamRepository)) as ITeamRepository
                       ?? throw new InvalidOperationException("ITeamRepository is not registered in the ValidationContext service provider.");

            var team = await repo.FindByNameAsync(name, ct).ConfigureAwait(false);
            return team != null;
        }
    }

    // Check participant username
    public class CheckParticipantUsernameAttribute : UniqueNameAttribute
    {
        public CheckParticipantUsernameAttribute() : base(Localization.UserAlreadyExists) { }

        protected override async Task<bool> ExistsAsync(object? value, ValidationContext context, CancellationToken ct)
        {
            var username = value as string;
            if (string.IsNullOrWhiteSpace(username)) return false;

            var repo = context.GetService(typeof(IParticipantRepository)) as IParticipantRepository
                       ?? throw new InvalidOperationException("IParticipantRepository is not registered in the ValidationContext service provider.");

            var participant = await repo.FindByUsernameAsync(username, ct).ConfigureAwait(false);
            return participant != null;
        }
    }

    // Check event name (Event)
    public class CheckEventNameAttribute : UniqueNameAttribute
    {
        public CheckEventNameAttribute() : base(Localization.EventAlreadyExists) { }

        protected override async Task<bool> ExistsAsync(object? value, ValidationContext context, CancellationToken ct)
        {
            var name = value as string;
            if (string.IsNullOrWhiteSpace(name)) return false;

            var repo = context.GetService(typeof(IEventRepository)) as IEventRepository
                       ?? throw new InvalidOperationException("IEventRepository is not registered in the ValidationContext service provider.");

            var evt = await repo.FindByNameAsync(name, ct).ConfigureAwait(false);
            return evt != null;
        }
    }

    // Check quest by name in the same Event: you need to pass eventId property name in the same model.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckQuestNameAttribute : UniqueNameAttribute
    {
        private readonly string _eventIdPropertyName;

        public CheckQuestNameAttribute(string eventIdPropertyName = nameof(Quest.EventId)) : base("Quest with this name already exists in the event.")
        {
            _eventIdPropertyName = eventIdPropertyName;
        }

        protected override async Task<bool> ExistsAsync(object? value, ValidationContext context, CancellationToken ct)
        {
            var name = value as string;
            if (string.IsNullOrWhiteSpace(name)) return false;

            var eventIdProp = context.ObjectType.GetProperty(_eventIdPropertyName, BindingFlags.Public | BindingFlags.Instance)
                              ?? throw new InvalidOperationException($"Property '{_eventIdPropertyName}' not found on type {context.ObjectType.Name}.");

            var eventIdVal = eventIdProp.GetValue(context.ObjectInstance);
            if (eventIdVal == null) return false;

            if (!long.TryParse(eventIdVal.ToString(), out var eventId))
                throw new InvalidOperationException($"Property '{_eventIdPropertyName}' could not be converted to long.");

            var repo = context.GetService(typeof(IQuestRepository)) as IQuestRepository
                       ?? throw new InvalidOperationException("IQuestRepository is not registered in the ValidationContext service provider.");

            var existing = await repo.FindByNameAsync(eventId, name, ct).ConfigureAwait(false);
            return existing != null;
        }
    }

    // Check bank key
    public class CheckBankKeyAttribute : UniqueNameAttribute
    {
        public CheckBankKeyAttribute() : base("Bank with this key already exists.") { }

        protected override async Task<bool> ExistsAsync(object? value, ValidationContext context, CancellationToken ct)
        {
            var key = value as string;
            if (string.IsNullOrWhiteSpace(key)) return false;

            var repo = context.GetService(typeof(IBankRepository)) as IBankRepository
                       ?? throw new InvalidOperationException("IBankRepository is not registered in the ValidationContext service provider.");

            var bank = await repo.FindByKeyAsync(key, ct).ConfigureAwait(false);
            return bank != null;
        }
    }

    // Check KmmTeam by Id
    public class CheckKmmTeamExistsAttribute : UniqueNameAttribute
    {
        public CheckKmmTeamExistsAttribute() : base("KmmTeam already exists with this identity.") { }

        protected override async Task<bool> ExistsAsync(object? value, ValidationContext context, CancellationToken ct)
        {
            if (value == null) return false;
            if (!long.TryParse(value.ToString(), out var id)) return false;

            var repo = context.GetService(typeof(IKmmTeamRepository)) as IKmmTeamRepository
                       ?? throw new InvalidOperationException("IKmmTeamRepository is not registered in the ValidationContext service provider.");

            var team = await repo.GetByIdWithLogsAsync(id, ct).ConfigureAwait(false);
            return team != null;
        }
    }
}
