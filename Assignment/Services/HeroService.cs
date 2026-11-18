using AutoMapper;
using Assignment.Dtos.HeroDtos;
using Assignment.Entities;
using Assignment.GameLogic;
using Assignment.Repositories;
using Assignment.Results;

namespace Assignment.Services
{
    public class HeroService(IHeroRepository heroes, IHeroClassRepository classes, IMapper mapper) : IHeroService
    {
        public async Task<GenericResult<PaginatedList<HeroResponse>>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var page = await heroes.GetPagedAsync(pageNumber, pageSize, searchTerm);

            var response = new PaginatedList<HeroResponse>
            {
                PageNumber = page.PageNumber,
                PageSize = page.PageSize,
                TotalPages = page.TotalPages,
                TotalRecords = page.TotalRecords,
                Data = mapper.Map<IEnumerable<HeroResponse>>(page.Data ?? Array.Empty<Hero>())
            };

            return GenericResult<PaginatedList<HeroResponse>>.Success(response);
        }

        public async Task<GenericResult<IEnumerable<HeroResponse>>> GetMineAsync(string ownerId)
        {
            var list = await heroes.GetByOwnerAsync(ownerId);
            return GenericResult<IEnumerable<HeroResponse>>.Success(mapper.Map<IEnumerable<HeroResponse>>(list));
        }

        public async Task<GenericResult<IEnumerable<HeroResponse>>> GetLeaderboardAsync(int top)
        {
            var list = await heroes.GetTopAsync(Math.Clamp(top, 1, 50));
            return GenericResult<IEnumerable<HeroResponse>>.Success(mapper.Map<IEnumerable<HeroResponse>>(list));
        }

        public async Task<GenericResult<HeroDetailsResponse>> GetByIdAsync(int id)
        {
            var hero = await heroes.GetWithDetailsAsync(id);

            if (hero is null)
                return GenericResult<HeroDetailsResponse>.Failure($"Hero {id} was not found.", ErrorKind.NotFound);

            return GenericResult<HeroDetailsResponse>.Success(mapper.Map<HeroDetailsResponse>(hero));
        }

        public async Task<GenericResult<HeroResponse>> CreateAsync(CreateHeroRequest request, string ownerId)
        {
            if (!await classes.ExistsAsync(request.HeroClassId))
                return GenericResult<HeroResponse>.Failure($"Hero class {request.HeroClassId} does not exist.");

            if (await heroes.CountByOwnerAsync(ownerId) >= GameRules.MaxHeroesPerPlayer)
                return GenericResult<HeroResponse>.Failure(
                    $"A player can have at most {GameRules.MaxHeroesPerPlayer} heroes.", ErrorKind.Conflict);

            var hero = mapper.Map<Hero>(request);
            hero.OwnerId = ownerId;
            await heroes.CreateAsync(hero);

            var created = await heroes.GetWithDetailsAsync(hero.Id);
            return GenericResult<HeroResponse>.Success(mapper.Map<HeroResponse>(created));
        }

        public async Task<Result> UpdateAsync(int id, UpdateHeroRequest request, string userId, bool isAdmin)
        {
            var hero = await heroes.GetByIdAsync(id);

            if (hero is null)
                return Result.Failure($"Hero {id} was not found.", ErrorKind.NotFound);

            if (!CanModify(hero, userId, isAdmin))
                return Result.Failure("You can only change your own heroes.", ErrorKind.Forbidden);

            if (!await classes.ExistsAsync(request.HeroClassId))
                return Result.Failure($"Hero class {request.HeroClassId} does not exist.");

            mapper.Map(request, hero);
            await heroes.UpdateAsync(hero);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, string userId, bool isAdmin)
        {
            var hero = await heroes.GetByIdAsync(id);

            if (hero is null)
                return Result.Failure($"Hero {id} was not found.", ErrorKind.NotFound);

            if (!CanModify(hero, userId, isAdmin))
                return Result.Failure("You can only delete your own heroes.", ErrorKind.Forbidden);

            await heroes.DeleteAsync(id);
            return Result.Success();
        }

        private static bool CanModify(Hero hero, string userId, bool isAdmin)
            => isAdmin || hero.OwnerId == userId;
    }
}
