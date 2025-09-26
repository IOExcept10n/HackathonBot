using System.ComponentModel.DataAnnotations.Schema;
using MyBots.Core.Persistence.DTO;
using MyBots.Modules.Common.Roles;

namespace HackathonBot.Models;

public enum RoleIndex
{
    Unknown,
    Participant,
    Organizer,
    Admin,
}

public class BotUserRole
{
    private string username = string.Empty;

    public long Id { get; set; }

    public long? TelegramId { get; set; }

    public string Username { get => username; set => username = value.Replace("@", "").ToLowerInvariant(); }
    public RoleIndex RoleId { get; set; }

    public string? Note { get; set; }

    public User? User { get; set; }

    [NotMapped]
    public Role Role
    {
        get => new(RoleId.ToString());
        set => RoleId = Enum.TryParse<RoleIndex>(value.Name, out var id) ? id : RoleIndex.Unknown;
    }
}
