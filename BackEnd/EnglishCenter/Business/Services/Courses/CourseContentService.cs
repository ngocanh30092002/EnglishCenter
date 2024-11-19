using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Courses
{
    public class CourseContentService : ICourseContentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly ILearningProcessService _processService;
        private readonly ICourseService _courseService;

        public CourseContentService(IMapper mapper, IUnitOfWork unit, ILearningProcessService processService, ICourseService courseService)
        {
            _mapper = mapper;
            _unit = unit;
            _processService = processService;
            _courseService = courseService;
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
            if (number <= 0)
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

        public async Task<Response> ChangeTypeAsync(long contentId, int type)
        {
            var courseContentModel = _unit.CourseContents.GetById(contentId);
            if (courseContentModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course contents",
                    Success = false
                };
            }

            if (!Enum.IsDefined(typeof(CourseContentTypeEnum), type))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid Type",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.CourseContents.ChangeTypeAsync(courseContentModel, (CourseContentTypeEnum)type);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change type failed",
                    Success = false
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> CreateAsync(CourseContentDto courseContentDto)
        {
            var courseModel = await _unit.Courses
                                        .Include(c => c.CourseContents)
                                        .FirstOrDefaultAsync(c => c.CourseId == courseContentDto.CourseId);
            if (courseModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any courses"
                };
            }

            if (!Enum.IsDefined(typeof(CourseContentTypeEnum), courseContentDto.Type!.Value))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid Type",
                    Success = false
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

            if (courseContentModel == null)
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
                Message = _mapper.Map<List<CourseContentResDto>>(courseContents),
                Success = true
            });
        }

        public Task<Response> GetAsync(long contentId)
        {
            var contentModel = _unit.CourseContents.GetById(contentId);

            return Task.FromResult(new Response()
            {
                Success = true,
                Message = _mapper.Map<CourseContentResDto>(contentModel),
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
                Message = _mapper.Map<List<CourseContentResDto>>(contents)
            };
        }

        public async Task<Response> GetHisCourseContentAsync(string courseId, long enrollId)
        {
            var courseModel = _unit.Courses.GetById(courseId);
            var enrollModel = _unit.Enrollment
                                   .Include(e => e.Class)
                                   .FirstOrDefault(e => e.EnrollId == enrollId);

            if (courseModel == null || enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = null,
                    Success = true
                };
            }

            var isQualified = await _courseService.CheckIsQualifiedAsync(enrollModel.UserId, courseModel.CourseId);

            var contents = await _unit.CourseContents.GetByCourseAsync(courseId);
            var contentRes = new List<CourseContentResDto>();

            if (isQualified.Success)
            {
                var isLocked = false;

                foreach (var content in contents)
                {
                    var resModel = _mapper.Map<CourseContentResDto>(content);

                    if (resModel.Type == 1)
                    {
                        foreach (var assignment in resModel.Assignments)
                        {
                            if (courseModel.IsSequential)
                            {
                                if (!isLocked)
                                {
                                    var status = await _processService.IsStatusLessonAsync(enrollModel, assignment.AssignmentId, null);

                                    assignment.Status = status.ToString();
                                    isLocked = status == LessonStatusEnum.Locked;

                                    continue;
                                }

                                assignment.Status = LessonStatusEnum.Locked.ToString();
                            }
                            else
                            {
                                var status = await _processService.IsStatusLessonAsync(enrollModel, assignment.AssignmentId, null);
                                assignment.Status = status.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (courseModel.IsSequential)
                        {
                            if (!isLocked)
                            {
                                var status = await _processService.IsStatusLessonAsync(enrollModel, null, resModel.Examination!.ExamId);

                                resModel.Examination.Status = status.ToString();
                                isLocked = status == LessonStatusEnum.Locked;

                                continue;
                            }

                            resModel.Examination!.Status = LessonStatusEnum.Locked.ToString();

                            // Todo: fake data to coding FE
                            resModel.Examination.Status = LessonStatusEnum.Open.ToString();

                        }
                        else
                        {
                            var status = await _processService.IsStatusLessonAsync(enrollModel, null, resModel.Examination!.ExamId);
                            resModel.Examination.Status = status.ToString();
                        }
                    }

                    contentRes.Add(resModel);
                }
            }
            else
            {
                contentRes = _mapper.Map<List<CourseContentResDto>>(contents);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = contentRes,
                Success = true
            };
        }

        public async Task<Response> GetTotalTimeByCourseAsync(string courseId)
        {
            var isExistCourse = _unit.Courses.IsExist(c => c.CourseId == courseId);
            if (!isExistCourse)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            var totalTime = await _unit.CourseContents.GetTotalTimeByCourseAsync(courseId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = totalTime,
                Success = true
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
