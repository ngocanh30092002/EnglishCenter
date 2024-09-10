using System.Globalization;
using AutoMapper;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Models.IdentityModel;

namespace EnglishCenter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Notification, NotiDtoModel>()
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => src.Time))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(des => des.LinkUrl, opt => opt.MapFrom(src => src.LinkUrl));

            CreateMap<NotiDtoModel, Notification>()
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(des => des.LinkUrl, opt => opt.MapFrom(src => src.LinkUrl))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<CourseDtoModel, Course>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.NumLesson, opt => opt.MapFrom(src => src.NumLesson))
                .ForMember(des => des.EntryPoint, opt => opt.MapFrom(src => src.EntryPoint))
                .ForMember(des => des.StandardPoint, opt => opt.MapFrom(src => src.StandardPoint))
                .ForMember(des => des.Priority, opt => opt.MapFrom(src => src.Priority))
                .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId));


            CreateMap<Course, CourseDtoModel>()
               .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(des => des.NumLesson, opt => opt.MapFrom(src => src.NumLesson))
               .ForMember(des => des.EntryPoint, opt => opt.MapFrom(src => src.EntryPoint))
               .ForMember(des => des.StandardPoint, opt => opt.MapFrom(src => src.StandardPoint))
               .ForMember(des => des.Priority, opt => opt.MapFrom(src => src.Priority))
               .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
               .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.Image.Replace("\\", "/") ?? ""))
               .ForMember(des => des.Image, opt => opt.MapFrom(src => (IFormFile?) null));

            CreateMap<UserInfoDtoModel, Student>()
                .ForMember(des => des.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(des => des.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(des => des.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(des => des.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(des => des.Address, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();

            CreateMap<Student, UserBackgroundDtoModel>()
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image.Replace("\\", "/") ?? ""))
                .ForMember(des => des.BackgroundImage, opt => opt.MapFrom(src => src.BackgroundImage.Replace("\\", "/") ?? ""));

            CreateMap<EventDtoModel, ScheduleEvent>()
                .ForMember(des => des.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.StartTime, opt => opt.MapFrom((src, des) => {
                    DateTime dateTime = DateTime.ParseExact(src.StartTime, "hh:mm tt", CultureInfo.InvariantCulture);
                    return TimeOnly.FromDateTime(dateTime);
                }))
                .ForMember(des => des.EndTime, opt => opt.MapFrom((src, des) =>
                {
                    DateTime dateTime = DateTime.ParseExact(src.EndTime, "hh:mm tt", CultureInfo.InvariantCulture);
                    return TimeOnly.FromDateTime(dateTime);
                }))
                .ForMember(des => des.Date, opt => opt.MapFrom(src => src.Date));
               
            CreateMap<ScheduleEvent, EventDtoModel>()
                .ForMember(des => des.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.StartTime, opt => opt.MapFrom( src => src.StartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)))
                .ForMember(des => des.EndTime, opt => opt.MapFrom(src => src.EndTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)))
                .ForMember(des => des.Date, opt => opt.MapFrom(src => src.Date));
        }
    }
}
