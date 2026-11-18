namespace Assignment.Entities
{
    // One-to-Many: HeroClass -> Heroes
    public class HeroClass : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int BonusPower { get; set; }

        public virtual ICollection<Hero> Heroes { get; set; } = new List<Hero>();
    }
}
