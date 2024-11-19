using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
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
                .ForMember(des => des.ExamId, opt => opt.MapFrom(src => src.ExamId))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(des => des.StartTime, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(des => des.EndTime, opt => opt.MapFrom(src => (DateTime?)null))
                .ReverseMap();

            CreateMap<LearningProcess, LearningProcessResDto>()
                .ForMember(des => des.ProcessId, opt => opt.MapFrom(src => src.ProcessId))
                .ForMember(des => des.EnrollId, opt => opt.MapFrom(src => src.EnrollId))
                .ForMember(des => des.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
                .ForMember(des => des.ExamId, opt => opt.MapFrom(src => src.ExamId))
                .ForMember(des => des.EnrollId, opt => opt.MapFrom(src => src.EnrollId))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => ((ProcessStatusEnum)src.Status).ToString()))
                .ForMember(des => des.StartTime, opt => opt.MapFrom(src => src.StartTime.ToString("HH:mm:ss dd/MM/yyyy")))
                .ForMember(des => des.EndTime, opt => opt.MapFrom(src => src.EndTime.HasValue ? src.EndTime.Value.ToString("HH:mm:ss dd/MM/yyyy") : null));

            CreateMap<AssignRecordDto, AssignmentRecord>()
                .ForMember(des => des.LearningProcessId, opt => opt.MapFrom(src => src.ProcessId))
                .ForMember(des => des.AssignQuesId, opt => opt.MapFrom(src => src.AssignQuesId))
                .ForMember(des => des.SubQueId, opt => opt.MapFrom(src => src.SubId))
                .ForMember(des => des.SelectedAnswer, opt => opt.MapFrom(src => src.SelectedAnswer))
                .ForMember(des => des.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect));

            CreateMap<AssignmentRecord, AssignmentRecordResDto>()
                .ForMember(des => des.AssignQuesId, opt => opt.MapFrom(src => src.AssignQuesId))
                .ForMember(des => des.SubQueId, opt => opt.MapFrom(src => src.SubQueId))
                .ForMember(des => des.SelectedAnswer, opt => opt.MapFrom(src => src.SelectedAnswer))
                .ForMember(des => des.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect));


            CreateMap<ToeicRecordDto, ToeicRecord>()
                .ForMember(des => des.LearningProcessId, opt => opt.MapFrom(src => src.ProcessId))
                .ForMember(des => des.SubQueId, opt => opt.MapFrom(src => src.SubId))
                .ForMember(des => des.SelectedAnswer, opt => opt.MapFrom(src => src.SelectedAnswer))
                .ForMember(des => des.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect));

            CreateMap<ToeicRecord, ToeicRecordResDto>()
               .ForMember(des => des.SubQueId, opt => opt.MapFrom(src => src.SubQueId))
               .ForMember(des => des.SelectedAnswer, opt => opt.MapFrom(src => src.SelectedAnswer))
               .ForMember(des => des.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
               .ForMember(des => des.AnswerInfo, opt => opt.MapFrom(src => src.SubToeic.Answer));

            CreateMap<ToeicPracticeRecord, ToeicPracticeRecordResDto>()
               .ForMember(des => des.SubQueId, opt => opt.MapFrom(src => src.SubQueId))
               .ForMember(des => des.SelectedAnswer, opt => opt.MapFrom(src => src.SelectedAnswer))
               .ForMember(des => des.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
               .ForMember(des => des.AnswerInfo, opt => opt.MapFrom(src => src.SubToeic.Answer));

            CreateMap<ToeicAttempt, ToeicAttemptResDto>()
               .ForMember(des => des.AttemptId, opt => opt.MapFrom(src => src.AttemptId))
               .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.UserId))
               .ForMember(des => des.ToeicExam, opt => opt.MapFrom(src => src.ToeicExam))
               .ForMember(des => des.ToeicId, opt => opt.MapFrom(src => src.ToeicId))
               .ForMember(des => des.Listening_Score, opt => opt.MapFrom(src => src.ListeningScore))
               .ForMember(des => des.Reading_Score, opt => opt.MapFrom(src => src.ReadingScore))
               .ForMember(des => des.Date, opt => opt.MapFrom(src => src.Date));

        }
    }
}
