using Assignment.Dtos.HeroDtos;
using Assignment.Results;

namespace Assignment.Services
{
    public interface IHeroService
    {
        Task<GenericResult<PaginatedList<HeroResponse>>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<GenericResult<IEnumerable<HeroResponse>>> GetMineAsync(string ownerId);
        Task<GenericResult<IEnumerable<HeroResponse>>> GetLeaderboardAsync(int top);
        Task<GenericResult<HeroDetailsResponse>> GetByIdAsync(int id);
        Task<GenericResult<HeroResponse>> CreateAsync(CreateHeroRequest request, string ownerId);
        Task<Result> UpdateAsync(int id, UpdateHeroRequest request, string userId, bool isAdmin);
        Task<Result> DeleteAsync(int id, string userId, bool isAdmin);
    }
}
