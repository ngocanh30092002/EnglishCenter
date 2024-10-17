using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<AssignmentDto, Assignment>()
               .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum))
               .ForMember(des => des.Time, opt => opt.MapFrom(src => (TimeOnly?)null))
               .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(des => des.CourseContentId, opt => opt.MapFrom(src => src.ContentId));


            CreateMap<Assignment, AssignmentResDto>()
                .ForMember(des => des.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
                .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => src.Time))
                .ForMember(des => des.ExpectedTime, opt => opt.MapFrom(src => src.ExpectedTime))
                .ForMember(des => des.Achieved_Percentage, opt => opt.MapFrom(src => src.AchievedPercentage))
                .ForMember(des => des.AssignQues, opt => opt.MapFrom(src => src.AssignQues));
        }
    }
}
