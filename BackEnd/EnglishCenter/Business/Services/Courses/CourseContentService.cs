using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Courses
{
    public class CourseContentService : ICourseContentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;

        public CourseContentService(IMapper mapper, IUnitOfWork unit) 
        {
            _mapper = mapper;
            _unit = unit;
        }
        public async Task<Response> ChangeContentAsync(long contentId, string content)
        {
            var courseContentModel = _unit.CourseContents.GetById(contentId);

            if (courseContentModel == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course contents"
                };
            }

            var isSuccess = await _unit.CourseContents.ChangeContentAsync(courseContentModel, content);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> ChangeNoNumAsync(long contentId, int number)
        {
            if(number <= 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "NoNum must be greater than 0"
                };
            }

            var courseContentModel = _unit.CourseContents.GetById(contentId);
            if (courseContentModel == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course contents"
                };
            }

            var isSuccess = await _unit.CourseContents.ChangeNoNumAsync(courseContentModel, number);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> CreateAsync(CourseContentDto courseContentDto)
        {
            var courseModel = await _unit.Courses
                                        .Include(c => c.CourseContents)
                                        .FirstOrDefaultAsync(c => c.CourseId == courseContentDto.CourseId);
            if(courseModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any courses"
                };
            }

            var currentNum = courseModel.CourseContents.Select(c => (int?)c.NoNum).Max();

            var courseContentModel = _mapper.Map<CourseContent>(courseContentDto);
            courseContentModel.NoNum = currentNum.HasValue ? currentNum.Value + 1 : 1;

            _unit.CourseContents.Add(courseContentModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> DeleteAsync(long contentId)
        {
            var courseContentModel = _unit.CourseContents.GetById(contentId);

            if(courseContentModel == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course contents"
                };
            }

            var maxNoNum = _unit.CourseContents
                                .Find(c => c.CourseId == courseContentModel.CourseId)
                                .Select(c => (int?)c.NoNum)
                                .Max();

            var isChangeSuccess = await _unit.CourseContents.ChangeNoNumAsync(courseContentModel, maxNoNum ?? 1);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't delete course content",
                    Success = false
                };
            }
                        

            _unit.CourseContents.Remove(courseContentModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public Task<Response> GetAllAsync()
        {
            var courseContents = _unit.CourseContents.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<CourseContentDto>>(courseContents),
                Success = true
            });
        }

        public Task<Response> GetAsync(long contentId)
        {
            var contentModel = _unit.CourseContents.GetById(contentId);

            return Task.FromResult(new Response()
            {
                Success = true,
                Message = _mapper.Map<CourseContentDto>(contentModel),
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }

        public async Task<Response> GetByCourseAsync(string courseId)
        {
            var contents = await _unit.CourseContents.GetByCourseAsync(courseId);

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<CourseContentDto>>(contents)
            };
        }

        public async Task<Response> UpdateAsync(long contentId, CourseContentDto courseContentDto)
        {
            var response = await _unit.CourseContents.UpdateAsync(contentId, courseContentDto);

            if (response.Success)
            {
                await _unit.CompleteAsync();
            }

            return response;
        }
    }
}
