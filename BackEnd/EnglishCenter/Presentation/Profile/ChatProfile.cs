using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<ChatFile, ChatFileResDto>()
                .ForMember(d => d.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(d => d.FilePath, opt => opt.MapFrom(src => src.FilePath.Replace("\\", "/")))
                .ForMember(d => d.FileType, opt => opt.MapFrom(src => src.FileType));

            CreateMap<ChatMessage, ChatMessageResDto>()
                .ForMember(d => d.SenderId, opt => opt.MapFrom(src => src.SenderId))
                .ForMember(d => d.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId))
                .ForMember(d => d.MessageId, opt => opt.MapFrom(src => src.MessageId))
                .ForMember(d => d.Message, opt => opt.MapFrom(src => src.Message))
                .ForMember(d => d.SendAt, opt => opt.MapFrom(src => src.SendAt.ToString("HH:mm:ss dd-MM-yyyy")))
                .ForMember(d => d.File, opt => opt.MapFrom(src => src.ChatFile))
                .ForMember(d => d.IsRead, opt => opt.MapFrom(src => src.IsRead))
                .ForMember(d => d.IsDelete, opt => opt.MapFrom(src => src.IsDelete));
        }
    }
}
