using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonBot.Models.Kmm;

public class Event
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int FirstPlaceReward { get; set; }
    public int SecondPlaceReward { get; set; }
    public int ThirdPlaceReward { get; set; }
    public ICollection<Quest> Quests { get; set; } = [];
    [NotMapped]
    public IEnumerable<Quest> Sabotages => Quests.Where(x => x.IsSabotage);
    [NotMapped]
    public IEnumerable<Quest> CitizenQuests => Quests.Where(x => !x.IsSabotage);
}
