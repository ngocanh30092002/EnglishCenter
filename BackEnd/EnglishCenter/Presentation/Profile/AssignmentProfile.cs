using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

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

            CreateMap<Assignment, AssignmentDto>()
                .ForMember(des => des.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
                .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => src.Time.ToString()))
            .ForMember(des => des.ContentId, opt => opt.MapFrom(src => src.CourseContentId));
        }
    }
}
