using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Assignment.Filters;
using Assignment.Services;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ServiceFilter(typeof(ExceptionHandlingFilter))]   // Session 05: exception filter -> ProblemDetails
    public class GameController(IGameService service) : ApiControllerBase
    {
        // Send a hero on a quest. The dice decide if it wins.
        [HttpPost("heroes/{heroId:int}/quests/{questId:int}/attempt")]
        public async Task<IActionResult> AttemptQuest(int heroId, int questId)
            => HandleResult(await service.AttemptQuestAsync(heroId, questId, UserId, IsAdmin));

        // Quests the hero can do right now (level is enough and not completed yet)
        [HttpGet("heroes/{heroId:int}/available-quests")]
        public async Task<IActionResult> AvailableQuests(int heroId)
            => HandleResult(await service.GetAvailableQuestsAsync(heroId, UserId, IsAdmin));
    }
}
