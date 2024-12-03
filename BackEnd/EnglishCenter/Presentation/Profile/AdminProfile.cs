using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<User, UserResDto>()
                .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(des => des.UserEmail, opt => opt.MapFrom(src => src.Email))
                .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(des => des.EmailConfirm, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(des => des.UserImage, opt => opt.MapFrom((src, des) =>
                {
                    if (src.Teacher != null) return src.Teacher.Image?.Replace("\\", "/");
                    if (src.Student != null) return src.Student.Image?.Replace("\\", "/");
                    return null;
                }))
                .ForMember(des => des.Lock, opt => opt.MapFrom((src, des) =>
                {
                    if (src.LockoutEnd > DateTime.Now) return 1;
                    return 0;
                }))
                .ForMember(des => des.DateOfBirth, opt => opt.MapFrom((src, des) =>
                {
                    if (src.Teacher != null) return src.Teacher.DateOfBirth == null ? null : src.Teacher.DateOfBirth.Value.ToString("MM/dd/yyyy");
                    if (src.Student != null) return src.Student.DateOfBirth == null ? null : src.Student.DateOfBirth.Value.ToString("MM/dd/yyyy");
                    return null;
                }))
                .ForMember(des => des.Address, opt => opt.MapFrom((src, des) =>
                {
                    if (src.Teacher != null) return src.Teacher.Address;
                    if (src.Student != null) return src.Student.Address;
                    return null;
                }));


            // Todo: Teacher update background => add role to update background
        }
    }
}
