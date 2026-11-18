using AutoMapper;
using Assignment.Dtos.GameDtos;
using Assignment.Dtos.HeroDtos;
using Assignment.Dtos.QuestDtos;
using Assignment.Entities;
using Assignment.GameLogic;
using Assignment.Repositories;
using Assignment.Results;

namespace Assignment.Services
{
    public class GameService(
        IHeroRepository heroes,
        IGenericRepository<Quest> quests,
        IHeroQuestRepository heroQuests,
        IDice dice,
        IMapper mapper) : IGameService
    {
        public async Task<GenericResult<AttemptQuestResponse>> AttemptQuestAsync(int heroId, int questId, string userId, bool isAdmin)
        {
            // Tracked on purpose: we change the hero and save everything once
            var hero = await heroes.GetWithDetailsAsync(heroId, asNoTracking: false);

            if (hero is null)
                return GenericResult<AttemptQuestResponse>.Failure($"Hero {heroId} was not found.", ErrorKind.NotFound);

            if (!isAdmin && hero.OwnerId != userId)
                return GenericResult<AttemptQuestResponse>.Failure("You can only play with your own heroes.", ErrorKind.Forbidden);

            var quest = await quests.GetByIdAsync(questId);

            if (quest is null)
                return GenericResult<AttemptQuestResponse>.Failure($"Quest {questId} was not found.", ErrorKind.NotFound);

            if (hero.Level < quest.RequiredLevel)
                return GenericResult<AttemptQuestResponse>.Failure(
                    $"{hero.Name} is level {hero.Level} but this quest needs level {quest.RequiredLevel}.");

            var heroQuest = hero.HeroQuests.FirstOrDefault(hq => hq.QuestId == questId);

            if (heroQuest is { Completed: true })
                return GenericResult<AttemptQuestResponse>.Failure("This quest is already completed.", ErrorKind.Conflict);

            if (heroQuest is null)
            {
                heroQuest = new HeroQuest { HeroId = hero.Id, QuestId = quest.Id };
                heroQuests.Add(heroQuest);
            }

            // Roll the dice
            var chance = GameRules.ChanceOfSuccess(hero.Level, hero.HeroClass.BonusPower, quest.Difficulty);
            var roll = dice.Roll();
            var victory = roll <= chance;

            heroQuest.Attempts++;

            var levelsGained = 0;
            if (victory)
            {
                heroQuest.Completed = true;
                heroQuest.CompletedAt = DateTime.UtcNow;
                levelsGained = GameRules.ApplyReward(hero, quest.RewardXp, quest.RewardGold);
            }

            await heroes.UpdateAsync(hero);   // one SaveChanges: hero + hero quest

            var message = victory
                ? $"Victory! +{quest.RewardXp} XP and +{quest.RewardGold} gold." + (levelsGained > 0 ? $" Level up! {hero.Name} is now level {hero.Level}." : string.Empty)
                : "Defeat... the quest was too tough this time. Try again!";

            var response = new AttemptQuestResponse
            {
                Victory = victory,
                Roll = roll,
                ChanceOfSuccess = chance,
                Message = message,
                XpGained = victory ? quest.RewardXp : 0,
                GoldGained = victory ? quest.RewardGold : 0,
                LevelsGained = levelsGained,
                Hero = mapper.Map<HeroResponse>(hero)
            };

            return GenericResult<AttemptQuestResponse>.Success(response);
        }

        public async Task<GenericResult<IEnumerable<QuestResponse>>> GetAvailableQuestsAsync(int heroId, string userId, bool isAdmin)
        {
            var hero = await heroes.GetWithDetailsAsync(heroId);

            if (hero is null)
                return GenericResult<IEnumerable<QuestResponse>>.Failure($"Hero {heroId} was not found.", ErrorKind.NotFound);

            if (!isAdmin && hero.OwnerId != userId)
                return GenericResult<IEnumerable<QuestResponse>>.Failure("You can only view your own heroes.", ErrorKind.Forbidden);

            var completed = hero.HeroQuests.Where(hq => hq.Completed).Select(hq => hq.QuestId).ToHashSet();

            var available = (await quests.GetAllAsync())
                .Where(q => q.RequiredLevel <= hero.Level && !completed.Contains(q.Id))
                .OrderBy(q => q.RequiredLevel)
                .ThenBy(q => q.Difficulty);

            return GenericResult<IEnumerable<QuestResponse>>.Success(mapper.Map<IEnumerable<QuestResponse>>(available));
        }
    }
}
