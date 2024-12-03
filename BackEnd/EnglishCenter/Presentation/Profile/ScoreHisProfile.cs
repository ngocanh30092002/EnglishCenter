using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

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

            CreateMap<ScoreHistory, ScoreHisResDto>()
               .ForMember(des => des.UserInfo, opt => opt.MapFrom(src => src.Enrollment.User))
               .ForMember(des => des.UserBackground, opt => opt.MapFrom(src => src.Enrollment.User))
               .ForMember(des => des.ScoreHisId, opt => opt.MapFrom(src => src.ScoreHisId))
               .ForMember(des => des.Entrance_Point, opt => opt.MapFrom(src => src.EntrancePoint))
               .ForMember(des => des.Midterm_Point, opt => opt.MapFrom(src => src.MidtermPoint))
               .ForMember(des => des.Final_Point, opt => opt.MapFrom(src => src.FinalPoint));
        }
    }
}
