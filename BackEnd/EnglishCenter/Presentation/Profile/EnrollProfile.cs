using AutoMapper;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation
{
    public class EnrollProfile : Profile
    {
        public EnrollProfile()
        {
            CreateMap<EnrollStatusDto, EnrollStatus>()
                        .ForMember(des => des.StatusId, opt => opt.MapFrom(src => src.StatusId))
                        .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
                        .ReverseMap();

            CreateMap<EnrollmentDto, Enrollment>()
                    .ForMember(des => des.EnrollId, opt => opt.MapFrom(src => src.EnrollId))
                    .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(des => des.ClassId, opt => opt.MapFrom(src => src.ClassId))
                    .ForMember(des => des.EnrollDate, opt => opt.MapFrom(src => src.EnrollDate))
                    .ForMember(des => des.StatusId, opt => opt.MapFrom(src => src.StatusId))
                    .ForMember(des => des.ScoreHisId, opt => opt.MapFrom(src => src.ScoreHisId))
                    .ForMember(des => des.UpdateTime, opt => opt.MapFrom(src => src.UpdateTime))
                    .ReverseMap();

            CreateMap<Enrollment, EnrollResDto>()
                .ForMember(des => des.EnrollId, opt => opt.MapFrom(src => src.EnrollId))
                .ForMember(des => des.EnrollDate, opt => opt.MapFrom(src => src.EnrollDate))
                .ForMember(des => des.Class, opt => opt.MapFrom(src => src.Class))
                .ForMember(des => des.Student, opt => opt.MapFrom(src => src.User))
                .ForMember(des => des.StudentBackground, opt => opt.MapFrom(src => src.User))
                .ForMember(des => des.EnrollStatus, opt => opt.MapFrom(src => src.Status.Name.ToString()))
                .ForMember(des => des.TeacherName, opt => opt.MapFrom((src, des) =>
                {
                    var fullName = src?.Class?.Teacher?.FirstName + " " + src?.Class?.Teacher?.LastName;
                    return fullName.Trim();
                }));

        }
    }
}
