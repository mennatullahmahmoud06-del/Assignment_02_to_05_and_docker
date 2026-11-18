using System.ComponentModel.DataAnnotations;

namespace Assignment.Dtos.HeroClassDtos
{
    public class HeroClassResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int BonusPower { get; set; }
        public int HeroesCount { get; set; }
    }

    public sealed record HeroClassRequest(
        [property: Required, StringLength(50, MinimumLength = 2)] string Name,
        [property: Range(0, 50)] int BonusPower,
        [property: StringLength(200)] string? Description = null);
}
