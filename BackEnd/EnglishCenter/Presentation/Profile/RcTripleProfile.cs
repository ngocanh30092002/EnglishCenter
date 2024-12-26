using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class RcTripleProfile : Profile
    {
        public RcTripleProfile()
        {
            CreateMap<QuesRcTriple, QuesRcTripleResDto>()
               .ForMember(des => des.Id, opt => opt.MapFrom(src => src.QuesId))
               .ForMember(des => des.Level, opt => opt.MapFrom(src => src.Level))
               .ForMember(des => des.ImageUrl_1, opt => opt.MapFrom(src => src.Image1.Replace("\\", "/")))
               .ForMember(des => des.ImageUrl_2, opt => opt.MapFrom(src => src.Image2.Replace("\\", "/")))
               .ForMember(des => des.ImageUrl_3, opt => opt.MapFrom(src => src.Image3.Replace("\\", "/")))
               .ForMember(des => des.Time, opt => opt.MapFrom(src => src.Time))
               .ForMember(des => des.Questions, opt => opt.MapFrom(src => src.SubRcTriples));


            CreateMap<AnswerRcTriple, AnswerRcTripleDto>()
                 .ForMember(des => des.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
                 .ForMember(des => des.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
                 .ForMember(des => des.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
                 .ForMember(des => des.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
                 .ForMember(des => des.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer))
                 .ForMember(des => des.Question, opt => opt.MapFrom(src => src.Question))
                 .ReverseMap();

            CreateMap<SubRcTripleDto, SubRcTriple>()
              .ForMember(des => des.PreQuesId, opt => opt.MapFrom(src => src.PreQuesId))
              .ForMember(des => des.Question, opt => opt.MapFrom(src => src.Question))
              .ForMember(des => des.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
              .ForMember(des => des.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
              .ForMember(des => des.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
              .ForMember(des => des.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
              .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum));

            CreateMap<SubRcTriple, SubRcTripleResDto>()
                 .ForMember(des => des.QuesId, opt => opt.MapFrom(src => src.SubId))
                 .ForMember(des => des.Question, opt => opt.MapFrom(src => src.Question))
                 .ForMember(des => des.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
                 .ForMember(des => des.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
                 .ForMember(des => des.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
                 .ForMember(des => des.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
                 .ForMember(des => des.AnswerInfo, opt => opt.MapFrom(src => src.Answer));
        }
    }
}
