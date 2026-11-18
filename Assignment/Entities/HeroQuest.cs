namespace Assignment.Entities
{
    // Join entity with a composite key (HeroId, QuestId) and extra data
    public class HeroQuest
    {
        public int HeroId { get; set; }
        public Hero Hero { get; set; } = null!;

        public int QuestId { get; set; }
        public Quest Quest { get; set; } = null!;

        public int Attempts { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
