using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<StudentInfoDto, Student>()
                  .ForMember(des => des.FirstName, opt => opt.MapFrom(src => src.FirstName))
                  .ForMember(des => des.LastName, opt => opt.MapFrom(src => src.LastName))
                  .ForMember(des => des.Gender, opt => opt.MapFrom(src => src.Gender))
                  .ForMember(des => des.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                  .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                  .ForMember(des => des.Address, opt => opt.MapFrom(src => src.Address))
                  .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.UserName))
                  .ReverseMap();

            CreateMap<Student, StudentBackgroundDto>()
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image == null ? "" : src.Image.Replace("\\", "/")))
                .ForMember(des => des.BackgroundImage, opt => opt.MapFrom(src => src.BackgroundImage == null ? "" : src.BackgroundImage.Replace("\\", "/")));
        }
    }
}
