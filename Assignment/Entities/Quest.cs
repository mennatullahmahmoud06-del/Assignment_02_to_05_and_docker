namespace Assignment.Entities
{
    public class Quest : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int RequiredLevel { get; set; } = 1;
        public int Difficulty { get; set; } = 1;
        public int RewardXp { get; set; }
        public int RewardGold { get; set; }

        // Many-to-Many: Quest <-> Hero (through HeroQuest)
        public virtual ICollection<HeroQuest> HeroQuests { get; set; } = new List<HeroQuest>();
        public virtual ICollection<Hero> Heroes { get; set; } = new List<Hero>();
    }
}
