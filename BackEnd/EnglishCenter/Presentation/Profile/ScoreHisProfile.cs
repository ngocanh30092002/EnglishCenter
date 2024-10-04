using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation
{
    public class ScoreHisProfile : Profile
    {
        public ScoreHisProfile() 
        {
            CreateMap<ScoreHistoryDto, ScoreHistory>()
               .ForMember(des => des.ScoreHisId, opt => opt.MapFrom(src => src.ScoreHisId))
               .ForMember(des => des.EntrancePoint, opt => opt.MapFrom(src => src.EntrancePoint))
               .ForMember(des => des.MidtermPoint, opt => opt.MapFrom(src => src.MidtermPoint))
               .ForMember(des => des.FinalPoint, opt => opt.MapFrom(src => src.FinalPoint))
               .ReverseMap();
        }
    }
}
