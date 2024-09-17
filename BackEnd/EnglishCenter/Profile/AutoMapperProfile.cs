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
            CreateMap<Notification, NotiDto>()
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => src.Time))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(des => des.LinkUrl, opt => opt.MapFrom(src => src.LinkUrl));

            CreateMap<NotiDto, Notification>()
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(des => des.LinkUrl, opt => opt.MapFrom(src => src.LinkUrl))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

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
               .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.Image.Replace("\\", "/") ?? ""))
               .ForMember(des => des.Image, opt => opt.MapFrom(src => (IFormFile?) null));

            CreateMap<UserInfoDto, Student>()
                .ForMember(des => des.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(des => des.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(des => des.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(des => des.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(des => des.Address, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();

            CreateMap<Student, UserBackgroundDto>()
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image.Replace("\\", "/") ?? ""))
                .ForMember(des => des.BackgroundImage, opt => opt.MapFrom(src => src.BackgroundImage.Replace("\\", "/") ?? ""));

            CreateMap<EventDto, ScheduleEvent>()
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
               
            CreateMap<ScheduleEvent, EventDto>()
                .ForMember(des => des.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.StartTime, opt => opt.MapFrom( src => src.StartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)))
                .ForMember(des => des.EndTime, opt => opt.MapFrom(src => src.EndTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)))
                .ForMember(des => des.Date, opt => opt.MapFrom(src => src.Date));

            CreateMap<CourseContentDto, CourseContent>()
                .ForMember(des => des.ContentId, opt => opt.MapFrom(src => src.ContentId))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum))
                .ForMember(des => des.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId));

            CreateMap<CourseContent, CourseContentDto>()
                .ForMember(des => des.ContentId, opt => opt.MapFrom(src => src.ContentId))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum))
                .ForMember(des => des.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(des => des.Assignments, opt => opt.MapFrom(src => src.Assignments.Select(e => new AssignmentDto()
                {
                    AssignmentId = e.AssignmentId,
                    ContentId = e.CourseContentId,
                    NoNum = e.NoNum,
                    Time = e.Time.Value.ToString("HH:mm:ss"),
                    Title = e.Title
                })));

            CreateMap<AssignmentDto, Assignment>()
                .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => (TimeOnly?) null))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.CourseContentId, opt => opt.MapFrom(src => src.ContentId));

            CreateMap<Assignment, AssignmentDto>()
                .ForMember(des => des.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
                .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => src.Time.ToString()))
                .ForMember(des => des.ContentId, opt => opt.MapFrom(src => src.CourseContentId));

            CreateMap<ClassDto, Class>()
                .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(des => des.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
                .ForMember(des => des.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(des => des.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(des => des.RegisteredNum, opt => opt.MapFrom(src => src.RegisteredNum))
                .ForMember(des => des.MaxNum, opt => opt.MapFrom(src => src.MaxNum));

            CreateMap<Class, ClassDto>()
                    .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                    .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
                    .ForMember(des => des.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
                    .ForMember(des => des.StartDate, opt => opt.MapFrom(src => src.StartDate))
                    .ForMember(des => des.EndDate, opt => opt.MapFrom(src => src.EndDate))
                    .ForMember(des => des.RegisteredNum, opt => opt.MapFrom(src => src.RegisteredNum))
                    .ForMember(des => des.MaxNum, opt => opt.MapFrom(src => src.MaxNum))
                    .ForMember(des => des.Image, opt => opt.MapFrom(src => (IFormFile?)null))
                    .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.Image.Replace("\\", "/")));

            CreateMap<EnrollStatusDto, EnrollStatus>()
                    .ForMember(des => des.StatusId, opt => opt.MapFrom(src => src.StatusId))
                    .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
                    .ReverseMap();

            CreateMap<EnrollmentDto, Enrollment>()
                    .ForMember(des => des.EnrollId, opt => opt.MapFrom(src => src.EnrollId))
                    .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                    .ForMember(des => des.EnrollDate, opt => opt.MapFrom(src => src.EnrollDate))
                    .ForMember(des => des.StatusId, opt => opt.MapFrom(src => src.StatusId))
                    .ReverseMap();

        }
    }
}
