using AutoMapper;
using Assignment.Dtos.QuestDtos;
using Assignment.Entities;
using Assignment.Repositories;
using Assignment.Results;

namespace Assignment.Services
{
    public class QuestService(IGenericRepository<Quest> repository, IMapper mapper) : IQuestService
    {
        public async Task<GenericResult<IEnumerable<QuestResponse>>> GetAllAsync()
        {
            var quests = await repository.GetAllAsync();
            var ordered = quests.OrderBy(q => q.RequiredLevel).ThenBy(q => q.Difficulty);
            return GenericResult<IEnumerable<QuestResponse>>.Success(mapper.Map<IEnumerable<QuestResponse>>(ordered));
        }

        public async Task<GenericResult<QuestResponse>> GetByIdAsync(int id)
        {
            var quest = await repository.GetByIdAsync(id);

            if (quest is null)
                return GenericResult<QuestResponse>.Failure($"Quest {id} was not found.", ErrorKind.NotFound);

            return GenericResult<QuestResponse>.Success(mapper.Map<QuestResponse>(quest));
        }

        public async Task<GenericResult<QuestResponse>> CreateAsync(QuestRequest request)
        {
            var entity = mapper.Map<Quest>(request);
            await repository.CreateAsync(entity);
            return GenericResult<QuestResponse>.Success(mapper.Map<QuestResponse>(entity));
        }

        public async Task<Result> UpdateAsync(int id, QuestRequest request)
        {
            var entity = await repository.GetByIdAsync(id);

            if (entity is null)
                return Result.Failure($"Quest {id} was not found.", ErrorKind.NotFound);

            mapper.Map(request, entity);
            await repository.UpdateAsync(entity);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var deleted = await repository.DeleteAsync(id);

            return deleted
                ? Result.Success()
                : Result.Failure($"Quest {id} was not found.", ErrorKind.NotFound);
        }
    }
}
