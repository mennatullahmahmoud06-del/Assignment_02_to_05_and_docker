using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Assignment.Constants;
using Assignment.Dtos.QuestDtos;
using Assignment.Services;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class QuestsController(IQuestService service) : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => HandleResult(await service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
            => HandleResult(await service.GetByIdAsync(id));

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create(QuestRequest request)
        {
            var result = await service.CreateAsync(request);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result)
                : HandleResult(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update(int id, QuestRequest request)
            => HandleResult(await service.UpdateAsync(id, request));

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
            => HandleResult(await service.DeleteAsync(id));
    }
}
