using Assignment.Dtos.QuestDtos;
using Assignment.Results;

namespace Assignment.Services
{
    public interface IQuestService
    {
        Task<GenericResult<IEnumerable<QuestResponse>>> GetAllAsync();
        Task<GenericResult<QuestResponse>> GetByIdAsync(int id);
        Task<GenericResult<QuestResponse>> CreateAsync(QuestRequest request);
        Task<Result> UpdateAsync(int id, QuestRequest request);
        Task<Result> DeleteAsync(int id);
    }
}
