using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class LcConversationProfile : Profile
    {
        public LcConversationProfile() 
        {
            CreateMap<QuesLcConversation, QuesLcConResDto>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.QuesId))
                .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.Image == null ? "" : src.Image.Replace("\\", "/")))
                .ForMember(des => des.AudioUrl, opt => opt.MapFrom(src => src.Audio.Replace("\\", "/")))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => src.Time))
                .ForMember(des => des.Questions, opt => opt.MapFrom(src => src.SubLcConversations));


            CreateMap<SubLcConDto, SubLcConversation>()
                .ForMember(des => des.PreQuesId, opt => opt.MapFrom(src => src.PreQuesId))
                .ForMember(des => des.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(des => des.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
                .ForMember(des => des.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
                .ForMember(des => des.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
                .ForMember(des => des.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
                .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum));

            CreateMap<SubLcConversation, SubLcConResDto>()
                 .ForMember(des => des.QuesId, opt => opt.MapFrom(src => src.SubId))
                 .ForMember(des => des.Question, opt => opt.MapFrom(src => src.Question))
                 .ForMember(des => des.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
                 .ForMember(des => des.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
                 .ForMember(des => des.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
                 .ForMember(des => des.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
                 .ForMember(des => des.AnswerInfo, opt => opt.MapFrom(src => src.Answer));


            CreateMap<AnswerLcConversation, AnswerLcConDto>()
                .ForMember(des => des.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
                .ForMember(des => des.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
                .ForMember(des => des.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
                .ForMember(des => des.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
                .ForMember(des => des.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer))
                .ForMember(des => des.Question, opt => opt.MapFrom(src => src.Question))
                .ReverseMap();
        }
    }
}
