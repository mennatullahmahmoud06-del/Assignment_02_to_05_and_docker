using System.ComponentModel.DataAnnotations;

namespace Assignment.Dtos.HeroDtos
{
    public class HeroResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Xp { get; set; }
        public int XpToNextLevel { get; set; }
        public int Gold { get; set; }
        public int Power { get; set; }
        public int HeroClassId { get; set; }
        public string HeroClassName { get; set; } = string.Empty;
    }

    public class HeroQuestResponse
    {
        public int QuestId { get; set; }
        public string QuestTitle { get; set; } = string.Empty;
        public int Attempts { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    // Hero + its quests (loaded through the navigation properties)
    public class HeroDetailsResponse : HeroResponse
    {
        public List<HeroQuestResponse> Quests { get; set; } = new();
    }

    public sealed record CreateHeroRequest(
        [property: Required, StringLength(50, MinimumLength = 2)] string Name,
        int HeroClassId);

    public sealed record UpdateHeroRequest(
        [property: Required, StringLength(50, MinimumLength = 2)] string Name,
        int HeroClassId);
}
