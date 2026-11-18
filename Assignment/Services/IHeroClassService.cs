using Assignment.Dtos.HeroClassDtos;
using Assignment.Results;

namespace Assignment.Services
{
    public interface IHeroClassService
    {
        Task<GenericResult<IEnumerable<HeroClassResponse>>> GetAllAsync();
        Task<GenericResult<HeroClassResponse>> GetByIdAsync(int id);
        Task<GenericResult<HeroClassResponse>> CreateAsync(HeroClassRequest request);
        Task<Result> UpdateAsync(int id, HeroClassRequest request);
        Task<Result> DeleteAsync(int id);
    }
}
