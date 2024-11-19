using System.Globalization;
using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile() 
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
        }
    }
}
