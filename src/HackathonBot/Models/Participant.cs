using System.ComponentModel.DataAnnotations.Schema;
using MyBots.Core.Persistence.DTO;

namespace HackathonBot.Models;

public class Participant
{
    private string nickname = string.Empty;

    public Guid Id { get; set; }

    public string Nickname { get => nickname; set => nickname = value.Replace("@", "").ToLowerInvariant(); }
    public long? TelegramId { get; set; }

    public User? FsmUser { get; set; }

    public string FullName { get; set; } = string.Empty;

    public Guid? TeamId { get; set; }

    public Team? Team { get; set; }

    public bool IsLeader { get; set; }

    [NotMapped]
    public bool IsLoggedIntoBot => TelegramId != null;
}
