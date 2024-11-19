using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Business.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public TeacherService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public Task<Response> GetFullNameAsync(string userId)
        {
            var teacher = _unit.Teachers.GetById(userId);

            if(teacher == null)
            {
                return Task.FromResult(new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any teachers",
                    Success = false,
                });
            }

            var fullName = _unit.Teachers.GetFullName(teacher);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = fullName,
                Success = true,
            });
        }

        public Task<Response> GetAsync(string userId) 
        {
            var teacherModel = _unit.Teachers.GetById(userId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<TeacherResDto>(teacherModel),
                Success = true
            });
        }
    }
}
