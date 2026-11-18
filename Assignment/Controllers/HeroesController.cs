using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Assignment.Dtos.HeroDtos;
using Assignment.Services;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class HeroesController(IHeroService service) : ApiControllerBase
    {
        // Pagination + search: GET /api/heroes?pageNumber=1&pageSize=10&searchTerm=ar
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
            => HandleResult(await service.GetAllAsync(pageNumber, pageSize, searchTerm));

        [HttpGet("mine")]
        public async Task<IActionResult> GetMine()
            => HandleResult(await service.GetMineAsync(UserId));

        [HttpGet("leaderboard")]
        public async Task<IActionResult> Leaderboard([FromQuery] int top = 10)
            => HandleResult(await service.GetLeaderboardAsync(top));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
            => HandleResult(await service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(CreateHeroRequest request)
        {
            var result = await service.CreateAsync(request, UserId);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result)
                : HandleResult(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateHeroRequest request)
            => HandleResult(await service.UpdateAsync(id, request, UserId, IsAdmin));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => HandleResult(await service.DeleteAsync(id, UserId, IsAdmin));
    }
}
