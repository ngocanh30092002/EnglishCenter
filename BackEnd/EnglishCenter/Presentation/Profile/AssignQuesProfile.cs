using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class AssignQuesProfile: Profile
    {
        public AssignQuesProfile() 
        {
            CreateMap<AssignQueDto, AssignQue>()
               .ForMember(des => des.Type, opt => opt.MapFrom(src => src.Type))
               .ForMember(des => des.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
               .ForMember(des => des.ImageQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Image ? src.QuesId : (long?)null))
               .ForMember(des => des.AudioQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Audio ? src.QuesId : (long?)null))
               .ForMember(des => des.ConversationQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Conversation ? src.QuesId : (long?)null))
               .ForMember(des => des.SingleQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Single ? src.QuesId : (long?)null))
               .ForMember(des => des.DoubleQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Double ? src.QuesId : (long?)null))
               .ForMember(des => des.TripleQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Triple ? src.QuesId : (long?)null));

            CreateMap<AssignQue, AssignQueResDto>()
               .ForMember(des => des.AssignQuesId, opt => opt.MapFrom(src => src.AssignQuesId))
               .ForMember(des => des.Type, opt => opt.MapFrom(src => ((QuesTypeEnum)src.Type).ToString()))
               .ForMember(des => des.QuesInfo, opt => opt.MapFrom((src, des) =>
               {
                   switch(src.Type)
                   {
                       case (int)QuesTypeEnum.Image:
                           return src.QuesImage;

                       case (int)QuesTypeEnum.Audio:
                           return src.QuesAudio;

                       case (int)QuesTypeEnum.Conversation:
                           return src.QuesConversation;

                       case (int)QuesTypeEnum.Single:
                           return src.QuesSingle;

                       case (int)QuesTypeEnum.Double:
                           return src.QuesDouble;

                       case (int)QuesTypeEnum.Triple:
                           return src.QuesTriple;
                   }

                   return (object?) null;
               }));

        }
    }
}
