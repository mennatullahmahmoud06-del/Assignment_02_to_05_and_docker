namespace Assignment.Entities
{
    public class Hero : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
        public int Xp { get; set; }
        public int Gold { get; set; }

        // Id of the player (Identity user). It lives in the Authentication database, so there is no real FK.
        public string OwnerId { get; set; } = string.Empty;

        // Many-to-One: Hero -> HeroClass
        public int HeroClassId { get; set; }
        public HeroClass HeroClass { get; set; } = null!;

        // Many-to-Many: Hero <-> Quest (through HeroQuest)
        public virtual ICollection<HeroQuest> HeroQuests { get; set; } = new List<HeroQuest>();
        public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();
    }
}
