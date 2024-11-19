using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class CourseContentProfile : Profile
    {
        public CourseContentProfile()
        {
            CreateMap<CourseContentDto, CourseContent>()
               .ForMember(des => des.ContentId, opt => opt.MapFrom(src => src.ContentId))
               .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum))
               .ForMember(des => des.Content, opt => opt.MapFrom(src => src.Content))
               .ForMember(des => des.Type, opt => opt.MapFrom(src => src.Type))
               .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId));

            CreateMap<CourseContent, CourseContentResDto>()
                .ForMember(des => des.ContentId, opt => opt.MapFrom(src => src.ContentId))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum))
                .ForMember(des => des.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(des => des.Assignments, opt => opt.MapFrom(src => src.Assignments))
                .ForMember(des => des.Examination, opt => opt.MapFrom(src => src.Examination));
        }
    }
}
