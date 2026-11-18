using AutoMapper;
using Assignment.Dtos.HeroClassDtos;
using Assignment.Entities;
using Assignment.Repositories;
using Assignment.Results;

namespace Assignment.Services
{
    public class HeroClassService(IHeroClassRepository repository, IMapper mapper) : IHeroClassService
    {
        public async Task<GenericResult<IEnumerable<HeroClassResponse>>> GetAllAsync()
        {
            var classes = await repository.GetAllWithHeroesAsync();
            var response = mapper.Map<IEnumerable<HeroClassResponse>>(classes);
            return GenericResult<IEnumerable<HeroClassResponse>>.Success(response);
        }

        public async Task<GenericResult<HeroClassResponse>> GetByIdAsync(int id)
        {
            var heroClass = await repository.GetByIdWithHeroesAsync(id);

            if (heroClass is null)
                return GenericResult<HeroClassResponse>.Failure($"Hero class {id} was not found.", ErrorKind.NotFound);

            return GenericResult<HeroClassResponse>.Success(mapper.Map<HeroClassResponse>(heroClass));
        }

        public async Task<GenericResult<HeroClassResponse>> CreateAsync(HeroClassRequest request)
        {
            if (await repository.NameExistsAsync(request.Name))
                return GenericResult<HeroClassResponse>.Failure($"A hero class named '{request.Name}' already exists.", ErrorKind.Conflict);

            var entity = mapper.Map<HeroClass>(request);
            await repository.CreateAsync(entity);

            return await GetByIdAsync(entity.Id);
        }

        public async Task<Result> UpdateAsync(int id, HeroClassRequest request)
        {
            var entity = await repository.GetByIdAsync(id);

            if (entity is null)
                return Result.Failure($"Hero class {id} was not found.", ErrorKind.NotFound);

            if (await repository.NameExistsAsync(request.Name, excludeId: id))
                return Result.Failure($"A hero class named '{request.Name}' already exists.", ErrorKind.Conflict);

            mapper.Map(request, entity);
            await repository.UpdateAsync(entity);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var entity = await repository.GetByIdWithHeroesAsync(id, asNoTracking: false);

            if (entity is null)
                return Result.Failure($"Hero class {id} was not found.", ErrorKind.NotFound);

            if (entity.Heroes.Count > 0)
                return Result.Failure("This class still has heroes. Delete or change them first.", ErrorKind.Conflict);

            await repository.DeleteAsync(id);
            return Result.Success();
        }
    }
}
