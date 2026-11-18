using Assignment.Entities;

namespace Assignment.GameLogic
{
    public static class GameRules
    {
        public const int MaxHeroesPerPlayer = 3;

        public static int XpForNextLevel(int level) => level * 100;

        public static int Power(int level, int classBonus) => level * 10 + classBonus;

        // 50% base + hero power - 15 per difficulty point, always between 5% and 95%
        public static int ChanceOfSuccess(int level, int classBonus, int difficulty)
            => Math.Clamp(50 + Power(level, classBonus) - difficulty * 15, 5, 95);

        // Adds the reward and levels the hero up as many times as the xp allows. Returns the levels gained.
        public static int ApplyReward(Hero hero, int xp, int gold)
        {
            hero.Xp += xp;
            hero.Gold += gold;

            var levelsGained = 0;
            while (hero.Xp >= XpForNextLevel(hero.Level))
            {
                hero.Xp -= XpForNextLevel(hero.Level);
                hero.Level++;
                levelsGained++;
            }

            return levelsGained;
        }
    }
}
