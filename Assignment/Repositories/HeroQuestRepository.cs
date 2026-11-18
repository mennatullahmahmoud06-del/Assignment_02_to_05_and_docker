using Assignment.Data;
using Assignment.Entities;

namespace Assignment.Repositories
{
    public class HeroQuestRepository : IHeroQuestRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HeroQuestRepository(ApplicationDbContext dbContext)
            => _dbContext = dbContext;

        public void Add(HeroQuest heroQuest)
            => _dbContext.HeroQuests.Add(heroQuest);
    }
}
