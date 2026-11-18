using Assignment.Dtos.HeroDtos;

namespace Assignment.Dtos.GameDtos
{
    public class AttemptQuestResponse
    {
        public bool Victory { get; set; }
        public int Roll { get; set; }
        public int ChanceOfSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int XpGained { get; set; }
        public int GoldGained { get; set; }
        public int LevelsGained { get; set; }
        public HeroResponse Hero { get; set; } = null!;
    }
}
