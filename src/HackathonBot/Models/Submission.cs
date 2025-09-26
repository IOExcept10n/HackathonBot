namespace HackathonBot.Models
{
    public class Submission
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public required Team Team { get; set; }
        public Case Case { get; set; }
        public string RepoUrl { get; set; } = string.Empty;
        public string? PresentationFileUrl { get; set; }
        public string? PresentationLink { get; set; }
        public DateTime SubmittedAt { get; set; }
        public Guid SubmittedById { get; set; }    // who uploaded Telegram ID
        public required Participant SubmittedBy { get; set; }
    }
}
