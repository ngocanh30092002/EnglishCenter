using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            CreateMap<ExaminationDto, Examination>()
               .ForMember(des => des.ContentId, opt => opt.MapFrom(src => src.ContentId))
               .ForMember(des => des.ToeicId, opt => opt.MapFrom(src => src.ToeicId))
               .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(des => des.Time, opt => opt.MapFrom(src => TimeOnly.MinValue))
               .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Examination, ExaminationDto>()
               .ForMember(des => des.ContentId, opt => opt.MapFrom(src => src.ContentId))
               .ForMember(des => des.ToeicId, opt => opt.MapFrom(src => src.ToeicId))
               .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(des => des.Time, opt => opt.MapFrom(src => src.Time.ToString("hh:mm:ss")))
               .ForMember(des => des.Status, opt => opt.MapFrom(src => ExerciseStatusEnum.Locked.ToString()))
               .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description));


            CreateMap<ToeicConversionDto, ToeicConversion>()
                .ForMember(des => des.NumberCorrect, opt => opt.MapFrom(src => src.NumberCorrect))
                .ForMember(des => des.EstimatedScore, opt => opt.MapFrom(src => src.EstimatedScore))
                .ForMember(des => des.Section, opt => opt.MapFrom(src => src.Section.ToString()))
                .ReverseMap();

            CreateMap<ToeicExamDto, ToeicExam>()
               .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(des => des.Code, opt => opt.MapFrom(src => src.Code))
               .ForMember(des => des.Year, opt => opt.MapFrom(src => src.Year))
               .ForMember(des => des.CompletedNum, opt => opt.MapFrom(src => src.Completed_Num))
               .ForMember(des => des.Point, opt => opt.MapFrom(src => src.Point))
               .ForMember(des => des.TimeMinutes, opt => opt.MapFrom(src => src.TimeMinutes))
               .ReverseMap();

            CreateMap<ToeicExam, ToeicExamResDto>()
               .ForMember(des => des.ToeicId, opt => opt.MapFrom(src => src.ToeicId))
               .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(des => des.Code, opt => opt.MapFrom(src => src.Code))
               .ForMember(des => des.Year, opt => opt.MapFrom(src => src.Year))
               .ForMember(des => des.Completed_Num, opt => opt.MapFrom(src => src.CompletedNum))
               .ForMember(des => des.Point, opt => opt.MapFrom(src => src.Point))
               .ForMember(des => des.TimeMinutes, opt => opt.MapFrom(src => src.TimeMinutes));

            CreateMap<QuesToeic, QuesToeicResDto>()
               .ForMember(des => des.QuesId, opt => opt.MapFrom(src => src.QuesId))
               .ForMember(des => des.ToeicId, opt => opt.MapFrom(src => src.ToeicId))
               .ForMember(des => des.IsGroup, opt => opt.MapFrom(src => src.IsGroup))
               .ForMember(des => des.Part, opt => opt.MapFrom(src => src.Part))
               .ForMember(des => des.Level, opt => opt.MapFrom(src => src.Level))
               .ForMember(des => des.Part_Name, opt => opt.MapFrom(src => ((PartEnum)src.Part).ToString()))
               .ForMember(des => des.Audio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Audio) ? "" : src.Audio.Replace("\\", "/")))
               .ForMember(des => des.Image_1, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Image_1) ? "" : src.Image_1.Replace("\\", "/")))
               .ForMember(des => des.Image_2, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Image_2) ? "" : src.Image_2.Replace("\\", "/")))
               .ForMember(des => des.Image_3, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Image_3) ? "" : src.Image_3.Replace("\\", "/")))
               .ForMember(des => des.SubQues, opt => opt.MapFrom(src => src.SubToeicList));


            CreateMap<SubToeicDto, SubToeic>()
               .ForMember(des => des.QuesId, opt => opt.MapFrom(src => src.QuesId))
               .ForMember(des => des.QuesNo, opt => opt.MapFrom(src => src.QuesNo))
               .ForMember(des => des.Question, opt => opt.MapFrom(src => src.Question))
               .ForMember(des => des.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
               .ForMember(des => des.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
               .ForMember(des => des.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
               .ForMember(des => des.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
               .ForMember(des => des.AnswerId, opt => opt.MapFrom(src => src.AnswerId));

            CreateMap<SubToeic, SubToeicResDto>()
               .ForMember(des => des.SubId, opt => opt.MapFrom(src => src.SubId))
               .ForMember(des => des.QuesNo, opt => opt.MapFrom(src => src.QuesNo))
               .ForMember(des => des.Question, opt => opt.MapFrom(src => src.Question))
               .ForMember(des => des.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
               .ForMember(des => des.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
               .ForMember(des => des.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
               .ForMember(des => des.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
               .ForMember(des => des.AnswerInfo, opt => opt.MapFrom(src => src.Answer));

            CreateMap<AnswerToeicDto, AnswerToeic>()
               .ForMember(des => des.Question, opt => opt.MapFrom(src => src.Question))
               .ForMember(des => des.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
               .ForMember(des => des.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
               .ForMember(des => des.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
               .ForMember(des => des.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
               .ForMember(des => des.Explanation, opt => opt.MapFrom(src => src.Explanation))
               .ForMember(des => des.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer))
               .ReverseMap();

            CreateMap<AnswerToeic, AnswerToeicResDto>()
               .ForMember(des => des.QuesNo, opt => opt.MapFrom(src => src.SubToeic.QuesNo))
               .ForMember(des => des.Question, opt => opt.MapFrom(src => src.Question))
               .ForMember(des => des.AnswerA, opt => opt.MapFrom(src => src.AnswerA))
               .ForMember(des => des.AnswerB, opt => opt.MapFrom(src => src.AnswerB))
               .ForMember(des => des.AnswerC, opt => opt.MapFrom(src => src.AnswerC))
               .ForMember(des => des.AnswerD, opt => opt.MapFrom(src => src.AnswerD))
               .ForMember(des => des.Explanation, opt => opt.MapFrom(src => src.Explanation))
               .ForMember(des => des.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer));


            CreateMap<ToeicDirectionDto, ToeicDirection>()
               .ForMember(des => des.Introduce_Listening, opt => opt.MapFrom(src => src.Introduce_Listening))
               .ForMember(des => des.Introduce_Reading, opt => opt.MapFrom(src => src.Introduce_Reading))
               .ForMember(des => des.Part1, opt => opt.MapFrom(src => src.Part_1))
               .ForMember(des => des.Part2, opt => opt.MapFrom(src => src.Part_2))
               .ForMember(des => des.Part3, opt => opt.MapFrom(src => src.Part_3))
               .ForMember(des => des.Part4, opt => opt.MapFrom(src => src.Part_4))
               .ForMember(des => des.Part5, opt => opt.MapFrom(src => src.Part_5))
               .ForMember(des => des.Part6, opt => opt.MapFrom(src => src.Part_6))
               .ForMember(des => des.Part7, opt => opt.MapFrom(src => src.Part_7));

            CreateMap<ToeicDirection, ToeicDirectionResDto>()
               .ForMember(des => des.Introduce_Listening, opt => opt.MapFrom(src => src.Introduce_Listening))
               .ForMember(des => des.Introduce_Reading, opt => opt.MapFrom(src => src.Introduce_Reading))
               .ForMember(des => des.Part1, opt => opt.MapFrom(src => src.Part1))
               .ForMember(des => des.Part2, opt => opt.MapFrom(src => src.Part2))
               .ForMember(des => des.Part3, opt => opt.MapFrom(src => src.Part3))
               .ForMember(des => des.Part4, opt => opt.MapFrom(src => src.Part4))
               .ForMember(des => des.Part5, opt => opt.MapFrom(src => src.Part5))
               .ForMember(des => des.Part6, opt => opt.MapFrom(src => src.Part6))
               .ForMember(des => des.Part7, opt => opt.MapFrom(src => src.Part7))
               .ForMember(des => des.Audio1, opt => opt.MapFrom(src => src.Audio1 == null ? "" : src.Audio1.Replace("\\", "/")))
               .ForMember(des => des.Audio2, opt => opt.MapFrom(src => src.Audio2 == null ? "" : src.Audio2.Replace("\\", "/")))
               .ForMember(des => des.Audio3, opt => opt.MapFrom(src => src.Audio3 == null ? "" : src.Audio3.Replace("\\", "/")))
               .ForMember(des => des.Audio4, opt => opt.MapFrom(src => src.Audio4 == null ? "" : src.Audio4.Replace("\\", "/")))
               .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image == null ? "" : src.Image.Replace("\\", "/")));


            CreateMap<RoadMapDto, RoadMap>()
               .ForMember(des => des.RoadMapId, opt => opt.MapFrom(src => src.RoadMapId))
               .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
               .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
               .ReverseMap();

            CreateMap<RoadMap, RoadMapResDto>()
              .ForMember(des => des.RoadMapId, opt => opt.MapFrom(src => src.RoadMapId))
              .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
              .ForMember(des => des.RoadMapName, opt => opt.MapFrom(src => src.Name))
              .ForMember(des => des.RoadMapExams, opt => opt.MapFrom(src => src.RoadMapExams));

            CreateMap<RoadMapExamDto, RoadMapExam>()
               .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(des => des.Point, opt => opt.MapFrom(src => src.Point))
               .ForMember(des => des.TimeMinutes, opt => opt.MapFrom(src => src.TimeMinutes))
               .ForMember(des => des.RoadMapId, opt => opt.MapFrom(src => src.RoadMapId));

            CreateMap<RoadMapExam, RoadMapExamResDto>()
               .ForMember(des => des.Id, opt => opt.MapFrom(src => src.RoadMapExamId))
               .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(des => des.DirectionId, opt => opt.MapFrom(src => src.DirectionId))
               .ForMember(des => des.Point, opt => opt.MapFrom(src => src.Point))
               .ForMember(des => des.Time_Minutes, opt => opt.MapFrom(src => src.TimeMinutes))
               .ForMember(des => des.Completed_Num, opt => opt.MapFrom(src => src.CompletedNum))
               .ForMember(des => des.RoadMapId, opt => opt.MapFrom(src => src.RoadMapId));

            CreateMap<RandomQuesToeicDto, RandomQuesToeic>()
              .ForMember(des => des.RoadMapExamId, opt => opt.MapFrom(src => src.RoadMapExamId))
              .ForMember(des => des.HomeworkId, opt => opt.MapFrom(src => src.HomeworkId))
              .ForMember(des => des.QuesToeicId, opt => opt.MapFrom(src => src.QuesToeicId))
              .ReverseMap();
        }
    }
}
