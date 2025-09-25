using HackathonBot.Properties;
using MyBots.Modules.Common;
using MyBots.Modules.Common.Interactivity;

namespace HackathonBot
{
    [LabelsStorage]
    internal static class Labels
    {
        public static ButtonLabel Hackathon { get; } = new(Emoji.PersonalComputer, Localization.HackathonModuleText);
        public static ButtonLabel Yes { get; } = new(Emoji.WhiteHeavyCheckMark, Localization.Yes);
        public static ButtonLabel No { get; } = new(Emoji.CrossMark, Localization.No);
    }
}
