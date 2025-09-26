using MyBots.Modules.Common.Roles;

namespace HackathonBot
{
    internal class Roles
    {
        public static Role Participant { get; } = new(nameof(Participant));
        public static Role Organizer { get; } = new(nameof(Organizer));
        public static Role Admin { get; } = new(nameof(Admin));
    }
}