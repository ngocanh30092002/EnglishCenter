using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation
{
    public class ToeicConversionProfile : Profile
    {
        public ToeicConversionProfile() 
        {
            CreateMap<ToeicConversionDto, ToeicConversion>()
                .ForMember(des => des.NumberCorrect, opt => opt.MapFrom(src => src.NumberCorrect))
                .ForMember(des => des.EstimatedScore, opt => opt.MapFrom(src => src.EstimatedScore))
                .ForMember(des => des.Section, opt => opt.MapFrom(src => src.Section.ToString()))
                .ReverseMap();
        }
    }
}
