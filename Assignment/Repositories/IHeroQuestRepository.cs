using Assignment.Entities;

namespace Assignment.Repositories
{
    // HeroQuest has a composite key (no BaseEntity), so it cannot use IGenericRepository<T>.
    // Add() does not save: the caller saves everything in one SaveChanges (one transaction).
    public interface IHeroQuestRepository
    {
        void Add(HeroQuest heroQuest);
    }
}
