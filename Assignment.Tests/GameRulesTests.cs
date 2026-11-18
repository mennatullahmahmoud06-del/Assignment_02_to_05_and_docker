using Assignment.Entities;
using Assignment.GameLogic;
using Xunit;

namespace Assignment.Tests;

public class GameRulesTests
{
    [Fact]
    public void XpForNextLevel_IsHundredPerLevel()
    {
        Assert.Equal(300, GameRules.XpForNextLevel(3));
    }

    [Fact]
    public void ChanceOfSuccess_IsAtLeastFivePercent()
    {
        Assert.Equal(5, GameRules.ChanceOfSuccess(level: 1, classBonus: 0, difficulty: 10));
    }

    [Fact]
    public void ChanceOfSuccess_IsAtMostNinetyFivePercent()
    {
        Assert.Equal(95, GameRules.ChanceOfSuccess(level: 10, classBonus: 10, difficulty: 1));
    }

    [Fact]
    public void ApplyReward_LevelsUpOnce()
    {
        var hero = new Hero { Level = 1, Xp = 0 };

        var levels = GameRules.ApplyReward(hero, xp: 250, gold: 10);

        Assert.Equal(1, levels);
        Assert.Equal(2, hero.Level);
        Assert.Equal(150, hero.Xp);
        Assert.Equal(10, hero.Gold);
    }

    [Fact]
    public void ApplyReward_CanLevelUpMoreThanOnce()
    {
        var hero = new Hero { Level = 1, Xp = 0 };

        var levels = GameRules.ApplyReward(hero, xp: 350, gold: 0);

        Assert.Equal(2, levels);
        Assert.Equal(3, hero.Level);
        Assert.Equal(50, hero.Xp);
    }
}
