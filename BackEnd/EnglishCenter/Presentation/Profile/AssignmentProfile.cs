using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<Assignment, AssignmentResDto>()
                .ForMember(des => des.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
                .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => src.Time.ToString("HH:mm:ss")))
                .ForMember(des => des.ExpectedTime, opt => opt.MapFrom(src => src.ExpectedTime.ToString("HH:mm:ss")))
                .ForMember(des => des.Achieved_Percentage, opt => opt.MapFrom(src => src.AchievedPercentage))
                .ForMember(des => des.CourseContentTitle, opt => opt.MapFrom(src => src.CourseContent.Title))
                .ForMember(des => des.AssignQues, opt => opt.MapFrom(src => src.AssignQues))
                .ForMember(des => des.CanViewResult, opt => opt.MapFrom(src => src.CanViewResult))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => ExerciseStatusEnum.Locked.ToString()));
        }
    }
}
