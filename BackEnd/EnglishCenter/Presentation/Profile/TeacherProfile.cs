using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, TeacherResDto>()
                .ForMember(des => des.FullName, opt => opt.MapFrom(src => (src.FirstName + " " + src.LastName).Trim()))
                .ForMember(des => des.Gender, opt => opt.MapFrom(src => (src.Gender == null ? Gender.Male : (Gender)src.Gender).ToString()))
                .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(des => des.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(des => des.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(des => des.BackgroundImageUrl, opt => opt.MapFrom(src => src.BackgroundImage == null ? "" : src.BackgroundImage.Replace("\\", "/")))
                .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.Image == null ? "" : src.Image.Replace("\\", "/")))
                .ForMember(des => des.UseName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(des => des.TeacherId, opt => opt.MapFrom(src => src.UserId));

        }
    }
}
