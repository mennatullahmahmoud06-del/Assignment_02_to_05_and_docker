using AutoMapper;
using Assignment.Dtos.HeroDtos;
using Assignment.Entities;
using Assignment.GameLogic;

namespace Assignment.Helper
{
    public class HeroProfile : Profile
    {
        public HeroProfile()
        {
            CreateMap<Hero, HeroResponse>()
                .ForMember(dest => dest.HeroClassName, opt => opt.MapFrom(src => src.HeroClass.Name))
                .ForMember(dest => dest.XpToNextLevel, opt => opt.MapFrom(src => GameRules.XpForNextLevel(src.Level)))
                .ForMember(dest => dest.Power, opt => opt.MapFrom(src => GameRules.Power(src.Level, src.HeroClass.BonusPower)));

            CreateMap<HeroQuest, HeroQuestResponse>()
                .ForMember(dest => dest.QuestTitle, opt => opt.MapFrom(src => src.Quest.Title));

            CreateMap<Hero, HeroDetailsResponse>()
                .IncludeBase<Hero, HeroResponse>()
                .ForMember(dest => dest.Quests, opt => opt.MapFrom(src => src.HeroQuests));

            CreateMap<CreateHeroRequest, Hero>();
            CreateMap<UpdateHeroRequest, Hero>();
        }
    }
}
