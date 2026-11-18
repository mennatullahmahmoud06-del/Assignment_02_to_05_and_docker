using AutoMapper;
using Assignment.Dtos.HeroClassDtos;
using Assignment.Entities;

namespace Assignment.Helper
{
    public class HeroClassProfile : Profile
    {
        public HeroClassProfile()
        {
            CreateMap<HeroClass, HeroClassResponse>()
                .ForMember(dest => dest.HeroesCount, opt => opt.MapFrom(src => src.Heroes.Count));

            CreateMap<HeroClassRequest, HeroClass>();
        }
    }
}
