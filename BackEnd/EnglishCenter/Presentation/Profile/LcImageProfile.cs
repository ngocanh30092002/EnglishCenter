using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class LcImageProfile : Profile
    {
        public LcImageProfile() 
        {
            CreateMap<AnswerLcImageDto, AnswerLcImage>()
                .ForMember(d => d.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
                .ForMember(d => d.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
                .ForMember(d => d.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
                .ForMember(d => d.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
                .ForMember(d => d.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer))
                .ReverseMap();

            CreateMap<QuesLcImage, QuesLcImageResDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.QuesId))
                .ForMember(d => d.ImageUrl, opt => opt.MapFrom(src => src.Image.Replace("\\", "/")))
                .ForMember(d => d.AudioUrl, opt => opt.MapFrom(src => src.Audio.Replace("\\", "/")))
                .ForMember(d => d.Time, opt => opt.MapFrom(src => src.Time))
                .ForMember(d => d.AnswerInfo, opt => opt.MapFrom(src => src.Answer));
        }
    }
}
