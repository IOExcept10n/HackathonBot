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
        public static ButtonLabel Notifications { get; } = new(Emoji.PublicAddressLoudspeaker, Localization.Notifications);
        public static ButtonLabel MailingToAll { get; } = new(Emoji.GlobeWithMeridians, Localization.SendToEveryone);
        public static ButtonLabel MailingToParticipants { get; } = new(Emoji.Bell, Localization.SendToAllParticipants);
        public static ButtonLabel MailingToOrganizers { get; } = new(Emoji.OpenMailboxWithRaisedFlag, Localization.SendToOrganizers);
        public static ButtonLabel MailingToTeam { get; } = new(Emoji.BustsInSilhouette, Localization.SendToTeam);
        public static ButtonLabel MailingPersonal { get; } = new(Emoji.Pistol, Localization.SendPersonal);
        public static ButtonLabel DeleteTeam { get; } = new(Emoji.DoNotLitterSymbol, Localization.DeleteTeam);
        public static ButtonLabel RenameTeam { get; } = new(Emoji.Bookmark, Localization.RenameTeam);
        public static ButtonLabel ChangeParticipantTeam { get; } = new(Emoji.AnticlockwiseDownwardsAndUpwardsOpenCircleArrows, Localization.ChangeParticipantTeam);
        public static ButtonLabel GetTeamSubmission { get; } = new(Emoji.PassportControl, Localization.GetTeamSubmission);
        public static ButtonLabel KmmManagement { get; } = new(Emoji.VideoGame, Localization.KmmManagement);
        public static ButtonLabel Kmm { get; } = new(Emoji.VideoGame, Localization.Kmm);
        public static ButtonLabel KmmTeamsStatus { get; } = new(Emoji.BlackTelephone, Localization.KmmTeamsStatus);
        public static ButtonLabel KmmCreateEvent { get; } = new(Emoji.TriangularFlagOnPost, Localization.KmmCreateEvent);
        public static ButtonLabel KmmCreateQuest { get; } = new(Emoji.CircusTent, Localization.KmmCreateQuest);
        public static ButtonLabel KmmManageEvents { get; } = new(Emoji.SatelliteAntenna, Localization.KmmManageEvents);
        public static ButtonLabel KmmManageQuests { get; } = new(Emoji.Scroll, Localization.KmmManageQuests);
        public static ButtonLabel KmmStartVote { get; } = new(Emoji.PoliceCarsRevolvingLight, Localization.KmmStartVote);
        public static ButtonLabel KmmAuditAbilities { get; } = new(Emoji.PageWithCurl, Localization.KmmAuditAbilities);
        public static ButtonLabel InitializeKmm { get; } = new(Emoji.SlotMachine, Localization.InitializeKmm);
        public static ButtonLabel StartEvent { get; } = new(Emoji.ChequeredFlag, Localization.StartEvent);
        public static ButtonLabel DeleteEvent { get; } = new(Emoji.CrossMark, Localization.DeleteEvent);
        public static ButtonLabel EditQuestName { get; } = new(Emoji.Notebook, Localization.EditQuestName);
        public static ButtonLabel EditQuestDescription { get; } = new(Emoji.OpenBook, Localization.EditQuestDescription);
        public static ButtonLabel DeleteQuest { get; } = new(Emoji.CrossMark, Localization.DeleteQuest);
    }
}
