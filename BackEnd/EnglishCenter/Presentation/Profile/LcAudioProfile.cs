using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class LcAudioProfile : Profile
    {
        public LcAudioProfile() 
        {
            CreateMap<AnswerLcAudioDto, AnswerLcAudio>()
               .ForMember(d => d.Question, opt => opt.MapFrom(src => src.Question))
               .ForMember(d => d.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
               .ForMember(d => d.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
               .ForMember(d => d.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
               .ForMember(d => d.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer))
               .ReverseMap();

            CreateMap<QuesLcAudio, QuesLcAudioResDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.QuesId))
                .ForMember(d => d.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(d => d.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
                .ForMember(d => d.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
                .ForMember(d => d.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
                .ForMember(d => d.AudioUrl, opt => opt.MapFrom(src => src.Audio.Replace("\\", "/")))
                .ForMember(d => d.Time, opt => opt.MapFrom(src => src.Time))
                .ForMember(d => d.AnswerInfo, opt => opt.MapFrom(src => src.Answer));


            CreateMap<QuesLcAudioDto, QuesLcAudio>()
                .ForMember(d => d.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(d => d.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
                .ForMember(d => d.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
                .ForMember(d => d.AnswerC, opt => opt.MapFrom(src => src.AnswerC));
        }
    }
}
