using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class LearningProcessProfile : Profile
    {
        public LearningProcessProfile() 
        {
            CreateMap<LearningProcessDto, LearningProcess>()
                .ForMember(des => des.EnrollId, opt => opt.MapFrom(src => src.EnrollId))
                .ForMember(des => des.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(des => des.StartTime, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(des => des.EndTime, opt => opt.MapFrom(src => (DateTime?)null))
                .ReverseMap();


            CreateMap<AnswerRecordDto, AnswerRecord>()
                .ForMember(des => des.LearningProcessId, opt => opt.MapFrom(src => src.ProcessId))
                .ForMember(des => des.AssignQuesId, opt => opt.MapFrom(src => src.AssignQuesId))
                .ForMember(des => des.SubQueId, opt => opt.MapFrom(src => src.SubId))
                .ForMember(des => des.SelectedAnswer, opt => opt.MapFrom(src => src.SelectedAnswer))
                .ForMember(des => des.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect));

            CreateMap<AnswerRecord, AnswerRecordResDto>()
                .ForMember(des => des.AssignQuesId, opt => opt.MapFrom(src => src.AssignQuesId))
                .ForMember(des => des.SubQueId, opt => opt.MapFrom(src => src.SubQueId))
                .ForMember(des => des.SelectedAnswer, opt => opt.MapFrom(src => src.SelectedAnswer))
                .ForMember(des => des.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect));
        }
    }
}
