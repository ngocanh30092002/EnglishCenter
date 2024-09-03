using AutoMapper;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Models.RequestModel;

namespace EnglishCenter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<NotiModel, Notification> ()
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(des => des.IsRead, opt => opt.MapFrom(src => false))
                .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(des => des.LinkUrl, opt => opt.MapFrom(src => src.LinkUrl))
                .ReverseMap();

            CreateMap<Notification, NotiDtoModel>()
                .ForMember(des => des.NotiId, opt => opt.MapFrom(src => src.NotiId))
                .ForMember(des => des.IsRead, opt => opt.MapFrom(src => src.IsRead))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => src.Time))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(des => des.LinkUrl, opt => opt.MapFrom(src => src.LinkUrl));
        }
    }
}
