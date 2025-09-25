using MyBots.Modules.Common.Roles;

namespace HackathonBot
{
    internal class Roles
    {
        public static Role User { get; } = new(nameof(User));
        public static Role Organizer { get; } = new(nameof(Organizer));
        public static Role Admin { get; } = new(nameof(Admin));
    }
}