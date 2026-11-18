using AutoMapper;
using Assignment.Dtos.QuestDtos;
using Assignment.Entities;

namespace Assignment.Helper
{
    public class QuestProfile : Profile
    {
        public QuestProfile()
        {
            CreateMap<Quest, QuestResponse>();
            CreateMap<QuestRequest, Quest>();
        }
    }
}
