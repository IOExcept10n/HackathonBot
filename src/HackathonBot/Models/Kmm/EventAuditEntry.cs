using MyBots.Core.Persistence.DTO;

namespace HackathonBot.Models.Kmm;

public enum EventType
{
    EventStarted,
    EventEnded,
    VoteStarted,
    VoteEnded
}

public class EventAuditEntry
{
    public long Id { get; set; }
    public long InitiatorId { get; set; }
    public User Initiator { get; set; } = null!;
    public string? Comment { get; set; }
    public EventType EventType { get; set; }
    public DateTime LoggedAt { get; set; }
}
