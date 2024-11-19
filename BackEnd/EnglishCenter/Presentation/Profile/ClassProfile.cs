using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<ClassDto, Class>()
                    .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                    .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
                    .ForMember(des => des.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
                    .ForMember(des => des.StartDate, opt => opt.MapFrom(src => src.StartDate))
                    .ForMember(des => des.EndDate, opt => opt.MapFrom(src => src.EndDate))
                    .ForMember(des => des.RegisteringNum, opt => opt.MapFrom(src => src.RegisteringNum))
                    .ForMember(des => des.RegisteredNum, opt => opt.MapFrom(src => src.RegisteredNum))
                    .ForMember(des => des.MaxNum, opt => opt.MapFrom(src => src.MaxNum))
                    .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Class, ClassResDto>()
                    .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                    .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
                    .ForMember(des => des.StartDate, opt => opt.MapFrom(src => src.StartDate))
                    .ForMember(des => des.EndDate, opt => opt.MapFrom(src => src.EndDate))
                    .ForMember(des => des.RegisteringNum, opt => opt.MapFrom(src => src.RegisteringNum))
                    .ForMember(des => des.RegisteredNum, opt => opt.MapFrom(src => src.RegisteredNum))
                    .ForMember(des => des.MaxNum, opt => opt.MapFrom(src => src.MaxNum))
                    .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.Image == null ? "" : src.Image.Replace("\\", "/")))
                    .ForMember(des => des.Status, opt => opt.MapFrom(src => ((ClassEnum)src.Status).ToString()))
                    .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(des => des.Teacher, opt => opt.MapFrom(src => src.Teacher));
        }
    }
}
