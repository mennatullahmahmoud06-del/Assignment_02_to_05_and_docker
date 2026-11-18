using System.ComponentModel.DataAnnotations;

namespace Assignment.Dtos.QuestDtos
{
    public class QuestResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int RequiredLevel { get; set; }
        public int Difficulty { get; set; }
        public int RewardXp { get; set; }
        public int RewardGold { get; set; }
    }

    public sealed record QuestRequest(
        [property: Required, StringLength(100, MinimumLength = 2)] string Title,
        [property: Range(1, 50)] int RequiredLevel,
        [property: Range(1, 10)] int Difficulty,
        [property: Range(0, 100000)] int RewardXp,
        [property: Range(0, 100000)] int RewardGold,
        [property: StringLength(300)] string? Description = null);
}
