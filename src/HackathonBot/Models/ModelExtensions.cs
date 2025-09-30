namespace HackathonBot.Models
{
    public static class ModelExtensions
    {
        public static string FormatDisplay(this Participant p) => $"{p.FullName} (@{p.Nickname})";

        public static string FormatDisplay(this BotUserRole r) => $"[{r.Role}] @{r.Username}";

        public static string GetNameOnly(this Participant participant) => participant.FullName.Split(' ').ElementAtOrDefault(1) ?? participant.FullName;

        public static string AsCanonicalNickname(this string nickname) => nickname.Replace("@", "").ToLowerInvariant();
    }
}