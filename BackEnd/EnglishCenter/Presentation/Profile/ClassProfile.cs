﻿using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<ClassDto, Class>()
                    .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                    .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
                    .ForMember(des => des.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
                    .ForMember(des => des.StartDate, opt => opt.MapFrom(src => src.StartDate))
                    .ForMember(des => des.EndDate, opt => opt.MapFrom(src => src.EndDate))
                    .ForMember(des => des.RegisteringNum, opt => opt.MapFrom(src => src.RegisteringNum))
                    .ForMember(des => des.RegisteredNum, opt => opt.MapFrom(src => src.RegisteredNum))
                    .ForMember(des => des.MaxNum, opt => opt.MapFrom(src => src.MaxNum))
                    .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Class, ClassResDto>()
                    .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                    .ForMember(des => des.CourseId, opt => opt.MapFrom(src => src.CourseId))
                    .ForMember(des => des.StartDate, opt => opt.MapFrom(src => src.StartDate))
                    .ForMember(des => des.EndDate, opt => opt.MapFrom(src => src.EndDate))
                    .ForMember(des => des.RegisteringNum, opt => opt.MapFrom(src => src.RegisteringNum))
                    .ForMember(des => des.RegisteredNum, opt => opt.MapFrom(src => src.RegisteredNum))
                    .ForMember(des => des.MaxNum, opt => opt.MapFrom(src => src.MaxNum))
                    .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.Image == null ? "" : src.Image.Replace("\\", "/")))
                    .ForMember(des => des.Status, opt => opt.MapFrom(src => ((ClassEnum)src.Status).ToString()))
                    .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(des => des.Teacher, opt => opt.MapFrom(src => src.Teacher));


            CreateMap<ClassScheduleDto, ClassSchedule>()
                    .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                    .ForMember(des => des.DayOfWeek, opt => opt.MapFrom(src => src.DayOfWeek))
                    .ForMember(des => des.StartPeriod, opt => opt.MapFrom(src => src.StartPeriod))
                    .ForMember(des => des.EndPeriod, opt => opt.MapFrom(src => src.EndPeriod))
                    .ForMember(des => des.ClassRoomId, opt => opt.MapFrom(src => src.ClassRoomId))
                    .ReverseMap();

            CreateMap<ClassRoomDto, ClassRoom>()
                    .ForMember(des => des.ClassRoomName, opt => opt.MapFrom(src => src.ClassRoomName))
                    .ForMember(des => des.Capacity, opt => opt.MapFrom(src => src.Capacity))
                    .ForMember(des => des.Location, opt => opt.MapFrom(src => src.Location))
                    .ReverseMap();

            CreateMap<LessonDto, Lesson>()
                    .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                    .ForMember(des => des.Date, opt => opt.MapFrom(src => DateOnly.Parse(src.Date)))
                    .ForMember(des => des.StartPeriod, opt => opt.MapFrom(src => src.StartPeriod))
                    .ForMember(des => des.EndPeriod, opt => opt.MapFrom(src => src.EndPeriod))
                    .ForMember(des => des.Topic, opt => opt.MapFrom(src => src.Topic))
                    .ForMember(des => des.ClassRoomId, opt => opt.MapFrom(src => src.ClassRoomId))
                    .ReverseMap();

            CreateMap<Lesson, LessonResDto>()
                    .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                    .ForMember(des => des.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")))
                    .ForMember(des => des.StartPeriod, opt => opt.MapFrom(src => src.StartPeriod))
                    .ForMember(des => des.EndPeriod, opt => opt.MapFrom(src => src.EndPeriod))
                    .ForMember(des => des.Topic, opt => opt.MapFrom(src => src.Topic))
                    .ForMember(des => des.DayOfWeek, opt => opt.MapFrom(src => src.Date.DayOfWeek.ToString()))
                    .ForMember(des => des.ClassRoom, opt => opt.MapFrom(src => src.ClassRoom));

            CreateMap<ClassMaterial, ClassMaterialResDto>()
                    .ForMember(des => des.ClassMaterialId, opt => opt.MapFrom(src => src.ClassMaterialId))
                    .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                    .ForMember(des => des.FilePath, opt => opt.MapFrom(src => src.FilePath.Replace("\\", "/")))
                    .ForMember(des => des.UploadAt, opt => opt.MapFrom(src => src.UploadAt.ToString("HH:mm:ss dd-MM-yyyy")))
                    .ForMember(des => des.UploadBy, opt => opt.MapFrom(src => src.UploadBy))
                    .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                    .ForMember(des => des.LessonInfo, opt => opt.MapFrom(src => src.Lesson));

            CreateMap<SubmissionFile, SubmissionFileResDto>()
                    .ForMember(des => des.FilePath, opt => opt.MapFrom(src => src.FilePath == null ? null : src.FilePath.Replace("\\", "/")))
                    .ForMember(des => des.LinkUrl, opt => opt.MapFrom(src => src.LinkUrl == null ? null : src.LinkUrl.Replace("\\", "/")))
                    .ForMember(des => des.Status, opt => opt.MapFrom(src => ((SubmissionFileEnum)src.Status).ToString()))
                    .ForMember(des => des.UploadAt, opt => opt.MapFrom(src => src.UploadAt.ToString("HH:mm:ss dd-MM-yyyy")))
                    .ForMember(des => des.UploadBy, opt => opt.MapFrom(src => src.UploadBy));

            CreateMap<SubmissionTask, SubmissionTaskResDto>()
                    .ForMember(des => des.SubmissionId, opt => opt.MapFrom(src => src.SubmissionId))
                    .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                    .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(des => des.StartTime, opt => opt.MapFrom(src => src.StartTime.ToString("HH:mm:ss dd-MM-yyyy")))
                    .ForMember(des => des.EndTime, opt => opt.MapFrom(src => src.EndTime.ToString("HH:mm:ss dd-MM-yyyy")))
                    .ForMember(des => des.LessonId, opt => opt.MapFrom(src => src.LessonId));

            CreateMap<Period, PeriodResDto>()
                    .ForMember(des => des.StartTime, opt => opt.MapFrom(src => src.StartTime))
                    .ForMember(des => des.EndTime, opt => opt.MapFrom(src => src.EndTime));

        }
    }
}
