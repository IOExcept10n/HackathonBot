using System.ComponentModel.DataAnnotations.Schema;

namespace MyBots.Core.Fsm.Persistency;

public class SessionState
{
    public long UserId { get; set; }
    public string StateId { get; set; } = string.Empty;

    [Column(TypeName = "jsonb")]
    public string? StateDataJson { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
    public int Version { get; set; }
    public List<StateHistoryEntry> History { get; set; } = [];
}

public class StateHistoryEntry
{
    public string StateId { get; set; } = string.Empty;
    public DateTimeOffset EnteredAt { get; set; }
}