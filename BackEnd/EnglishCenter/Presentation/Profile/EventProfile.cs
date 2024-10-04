using System.Globalization;
using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

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
                .ForMember(des => des.Date, opt => opt.MapFrom(src => src.Date));
        }
    }
}
