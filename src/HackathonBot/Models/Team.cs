namespace HackathonBot.Models;

public class Team
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Case Case { get; set; }

    public long KmmId { get; set; }

    public Guid? SubmissionId { get; set; }

    public Submission? Submission { get; set; }

    public ICollection<Participant> Members { get; set; } = [];
}
