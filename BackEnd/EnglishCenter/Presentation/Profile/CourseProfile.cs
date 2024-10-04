using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation
{
    public class CourseProfile : Profile
    {
        public CourseProfile() 
        {
            CreateMap<CourseDto, Course>()
               .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(des => des.NumLesson, opt => opt.MapFrom(src => src.NumLesson))
               .ForMember(des => des.EntryPoint, opt => opt.MapFrom(src => src.EntryPoint))
               .ForMember(des => des.StandardPoint, opt => opt.MapFrom(src => src.StandardPoint))
               .ForMember(des => des.Priority, opt => opt.MapFrom(src => src.Priority))
               .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId));

            CreateMap<Course, CourseDto>()
               .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(des => des.NumLesson, opt => opt.MapFrom(src => src.NumLesson))
               .ForMember(des => des.EntryPoint, opt => opt.MapFrom(src => src.EntryPoint))
               .ForMember(des => des.StandardPoint, opt => opt.MapFrom(src => src.StandardPoint))
               .ForMember(des => des.Priority, opt => opt.MapFrom(src => src.Priority))
               .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
               .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.Image == null ? "" : src.Image.Replace("\\", "/")))
               .ForMember(des => des.Image, opt => opt.MapFrom(src => (IFormFile?)null))
               .ForMember(des => des.ImageThumbnailUrl, opt => opt.MapFrom(src => src.ImageThumbnail == null ? "" : src.ImageThumbnail.Replace("\\", "/")))
               .ForMember(des => des.ImageThumbnail, opt => opt.MapFrom(src => (IFormFile?)null));
        }
    }
}
