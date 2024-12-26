using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class RcSentenceProfile : Profile
    {
        public RcSentenceProfile()
        {
            CreateMap<AnswerRcSentenceDto, AnswerRcSentence>()
                .ForMember(d => d.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
                .ForMember(d => d.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
                .ForMember(d => d.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
                .ForMember(d => d.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
                .ForMember(d => d.Explanation, opt => opt.MapFrom(src => src.Explanation))
                .ForMember(d => d.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(d => d.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer))
                .ReverseMap();

            CreateMap<QuesRcSentenceDto, QuesRcSentence>()
                .ForMember(d => d.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(d => d.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
                .ForMember(d => d.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
                .ForMember(d => d.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
                .ForMember(d => d.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
                .ForMember(d => d.Time, opt => opt.MapFrom(src => TimeOnly.MinValue));

            CreateMap<QuesRcSentence, QuesRcSentenceResDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.QuesId))
                .ForMember(d => d.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(d => d.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
                .ForMember(d => d.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
                .ForMember(d => d.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
                .ForMember(d => d.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
                .ForMember(d => d.Time, opt => opt.MapFrom(src => src.Time))
                .ForMember(d => d.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(d => d.AnswerInfo, opt => opt.MapFrom(src => src.Answer));
        }
    }
}
