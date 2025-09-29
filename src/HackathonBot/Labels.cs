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
        public static ButtonLabel Case { get; } = new(Emoji.Briefcase, Localization.Cases);
        public static ButtonLabel CaseLD { get; } = new(Emoji.Factory, Localization.CaseLD);
        public static ButtonLabel CaseTBank { get; } = new(Emoji.Bank, Localization.CaseTBank);
        public static ButtonLabel MyTeam { get; } = new(Emoji.GlowingStar, Localization.MyTeam);
        public static ButtonLabel UploadSolution { get; } = new(Emoji.FloppyDisk, Localization.UploadSolution);
        public static ButtonLabel Administration { get; } = new(Emoji.Key, Localization.AdminModuleText);
        public static ButtonLabel DeleteAdmin { get; } = new(Emoji.CrossMark, Localization.DeleteAdmin);
        public static ButtonLabel DeleteParticipant { get; } = new(Emoji.NegativeSquaredCrossMark, Localization.DeleteParticipant);
        public static ButtonLabel RegisterAdmin { get; } = new(Emoji.Memo, Localization.RegisterAdmin);
        public static ButtonLabel RegisterParticipant { get; } = new(Emoji.Scroll, Localization.RegisterParticipant);
        public static ButtonLabel Admin { get; } = new(Emoji.Lock, Localization.Admin);
        public static ButtonLabel Organizer { get; } = new(Emoji.Wrench, Localization.Organizer);
        public static ButtonLabel Back { get; } = new(Emoji.BackWithLeftwardsArrowAbove, Localization.Back);
        public static ButtonLabel ManageTeams { get; } = new(Emoji.Snowflake, Localization.ManageTeams);
        public static ButtonLabel ManageParticipants { get; } = new(Emoji.TriangularFlagOnPost, Localization.ShowParticipantsList);
        public static ButtonLabel ManageOrganizers { get; } = new(Emoji.ConstructionSign, Localization.ManageOrganizers);
        public static ButtonLabel UploadParticipantsList { get; } = new(Emoji.Envelope, Localization.UploadParticipantsList);
        public static ButtonLabel SelectCase { get; } = new(Emoji.Pencil, Localization.SelectCase);
        public static ButtonLabel UploadPresentation { get; } = new(Emoji.SunriseOverMountains, Localization.UploadPresentation);
        public static ButtonLabel UploadRepo { get; } = new(Emoji.Cloud, Localization.UploadRepo);
        public static ButtonLabel MySubmission { get; } = new(Emoji.Ledger, Localization.MySubmission);
    }
}

