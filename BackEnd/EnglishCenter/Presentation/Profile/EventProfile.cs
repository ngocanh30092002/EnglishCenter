using System.Globalization;
using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventDto, ScheduleEvent>()
                .ForMember(des => des.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.StartTime, opt => opt.MapFrom((src, des) =>
                {
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
                .ForMember(des => des.StartTime, opt => opt.MapFrom(src => src.StartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)))
                .ForMember(des => des.EndTime, opt => opt.MapFrom(src => src.EndTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)))
                .ForMember(des => des.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(des => des.IsSend, opt => opt.MapFrom(src => src.IsSend));

            CreateMap<IssueResponse, IssueResponseResDto>()
                .ForMember(des => des.IssueResId, opt => opt.MapFrom(src => src.IssueResId))
                .ForMember(des => des.IssueId, opt => opt.MapFrom(src => src.IssueId))
                .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(des => des.Message, opt => opt.MapFrom(src => src.Message))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("HH:mm:ss dd/MM/yyyy")));

            CreateMap<IssueReport, IssueReportResDto>()
               .ForMember(des => des.IssueId, opt => opt.MapFrom(src => src.IssueId))
               .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(des => des.Type, opt => opt.MapFrom(src => src.Type))
               .ForMember(des => des.TypeName, opt => opt.MapFrom(src => ((IssueTypeEnum)src.Type).ToString()))
               .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(des => des.StatusName, opt => opt.MapFrom(src => ((IssueStatusEnum)src.Status).ToString()))
               .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("HH:mm:ss dd/MM/yyyy")));

        }
    }
}
