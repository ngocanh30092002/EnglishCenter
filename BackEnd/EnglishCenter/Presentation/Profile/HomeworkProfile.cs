using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class HomeworkProfile : Profile
    {
        public HomeworkProfile() 
        {
            CreateMap<Homework, HomeworkResDto>()
                .ForMember(des => des.HomeworkId, opt => opt.MapFrom(src => src.HomeworkId))
                .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                .ForMember(des => des.StartTime, opt => opt.MapFrom(src => src.StartTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(des => des.EndTime, opt => opt.MapFrom(src => src.EndTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(des => des.LateSubmitDays, opt => opt.MapFrom(src => src.LateSubmitDays))
                .ForMember(des => des.Achieved_Percentage, opt => opt.MapFrom(src => src.AchievedPercentage))
                .ForMember(des => des.ExpectedTime, opt => opt.MapFrom(src => src.ExpectedTime))
                .ForMember(des => des.Time, opt => opt.MapFrom(src => src.Time))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title));


            CreateMap<HomeQueDto, HomeQue>()
               .ForMember(des => des.Type, opt => opt.MapFrom(src => src.Type))
               .ForMember(des => des.HomeworkId, opt => opt.MapFrom(src => src.HomeworkId))
               .ForMember(des => des.ImageQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Image ? src.QuesId : (long?)null))
               .ForMember(des => des.AudioQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Audio ? src.QuesId : (long?)null))
               .ForMember(des => des.ConversationQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Conversation ? src.QuesId : (long?)null))
               .ForMember(des => des.SingleQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Single ? src.QuesId : (long?)null))
               .ForMember(des => des.DoubleQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Double ? src.QuesId : (long?)null))
               .ForMember(des => des.TripleQuesId, opt => opt.MapFrom(src => src.Type == (int)QuesTypeEnum.Triple ? src.QuesId : (long?)null));

            CreateMap<HomeQue, HomeQueResDto>()
               .ForMember(des => des.HomeQuesId, opt => opt.MapFrom(src => src.HomeQuesId))
               .ForMember(des => des.NoNum, opt => opt.MapFrom(src => src.NoNum))
               .ForMember(des => des.Type, opt => opt.MapFrom(src => ((QuesTypeEnum)src.Type).ToString()))
               .ForMember(des => des.QuesInfo, opt => opt.MapFrom((src, des, index, context) =>
               {
                   switch (src.Type)
                   {
                       case (int)QuesTypeEnum.Image:
                           return context.Mapper.Map<QuesLcImageResDto>(src.QuesImage);

                       case (int)QuesTypeEnum.Audio:
                           return context.Mapper.Map<QuesLcAudioResDto>(src.QuesAudio);

                       case (int)QuesTypeEnum.Conversation:
                           return context.Mapper.Map<QuesLcConResDto>(src.QuesConversation);

                       case (int)QuesTypeEnum.Sentence:
                           return context.Mapper.Map<QuesRcSentenceResDto>(src.QuesSentence);

                       case (int)QuesTypeEnum.Single:
                           return context.Mapper.Map<QuesRcSingleResDto>(src.QuesSingle);

                       case (int)QuesTypeEnum.Double:
                           return context.Mapper.Map<QuesRcDoubleResDto>(src.QuesDouble);

                       case (int)QuesTypeEnum.Triple:
                           return context.Mapper.Map<QuesRcTripleResDto>(src.QuesTriple);
                   }

                   return (object?)null;
               }));

            CreateMap<HwSubmissionDto, HwSubmission>()
               .ForMember(des => des.HomeworkId, opt => opt.MapFrom(src => src.HomeworkId))
               .ForMember(des => des.Date, opt => opt.MapFrom(src => Convert.ToDateTime(src.Date)))
               .ForMember(des => des.FeedBack, opt => opt.MapFrom(src => src.Feedback))
               .ForMember(des => des.EnrollId, opt => opt.MapFrom(src => src.EnrollId))
               .ForMember(des => des.IsPass, opt => opt.MapFrom(src => src.IsPass ?? false))
               .ReverseMap();

            CreateMap<HwSubRecordDto, HwSubRecord>()
                .ForMember(des => des.SubmissionId, opt => opt.MapFrom(src => src.SubmissionId))
                .ForMember(des => des.HwQuesId, opt => opt.MapFrom(src => src.HwQuesId))
                .ForMember(des => des.HwSubQuesId, opt => opt.MapFrom(src => src.HwSubQuesId))
                .ForMember(des => des.SelectedAnswer, opt => opt.MapFrom(src => src.SelectedAnswer));

            CreateMap<HwSubRecord, HwSubRecordResDto>()
                .ForMember(des => des.HwQuesId, opt => opt.MapFrom(src => src.HwQuesId))
                .ForMember(des => des.HwSubQuesId, opt => opt.MapFrom(src => src.HwSubQuesId))
                .ForMember(des => des.SelectedAnswer, opt => opt.MapFrom(src => src.SelectedAnswer))
                .ForMember(des => des.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect));
        }
    }
}
