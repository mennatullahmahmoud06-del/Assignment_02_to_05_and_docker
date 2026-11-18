using Assignment.Dtos.GameDtos;
using Assignment.Dtos.QuestDtos;
using Assignment.Results;

namespace Assignment.Services
{
    public interface IGameService
    {
        Task<GenericResult<AttemptQuestResponse>> AttemptQuestAsync(int heroId, int questId, string userId, bool isAdmin);
        Task<GenericResult<IEnumerable<QuestResponse>>> GetAvailableQuestsAsync(int heroId, string userId, bool isAdmin);
    }
}
