using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class DictionaryProfile : Profile
    {
        public DictionaryProfile()
        {
            CreateMap<UserWord, UserWordResDto>()
                   .ForMember(des => des.UserWordId, opt => opt.MapFrom(src => src.UserWordId))
                   .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.UserId))
                   .ForMember(des => des.Word, opt => opt.MapFrom(src => src.Word))
                   .ForMember(des => des.Translation, opt => opt.MapFrom(src => src.Translation))
                   .ForMember(des => des.Phonetic, opt => opt.MapFrom(src => src.Phonetic))
                   .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image.Replace("\\", "/")))
                   .ForMember(des => des.Tag, opt => opt.MapFrom(src => src.Tag))
                   .ForMember(des => des.Type, opt => opt.MapFrom(src => ((WordTypeEnum)src.Type).ToString()))
                   .ForMember(des => des.IsFavorite, opt => opt.MapFrom(src => src.IsFavorite))
                   .ForMember(des => des.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.ToString("dd-MM-yyyy")));
        }
    }
}
