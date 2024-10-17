using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EnglishCenter.Business.Services.Courses
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;

        public AssignmentService(IMapper mapper, IUnitOfWork unit)
        {
            _mapper = mapper;
            _unit = unit;
        }

        public async Task<Response> ChangeCourseContentAsync(long assignmentId, long contentId)
        {
            var assignModel = _unit.Assignments.GetById(assignmentId);
            if(assignModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any assignments"
                };
            }

            var isExistCourseContent = _unit.CourseContents.IsExist(c => c.ContentId == contentId);
            if(!isExistCourseContent)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any course content"
                };
            }

            var isSuccess = await _unit.Assignments.ChangeCourseContentAsync(assignModel, contentId);
            if(!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
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

        public async Task<Response> ChangeNoNumAsync(long assignmentId, int number)
        {
            var assignModel = _unit.Assignments.GetById(assignmentId);
            if (assignModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any assignments"
                };
            }

            if (number <= 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "NoNum must be greater than 0"
                };
            }

            var isSuccess = await _unit.Assignments.ChangeNoNumberAsync(assignModel, number);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
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

        public async Task<Response> ChangeTimeAsync(long assignmentId, string time)
        {
            var assignModel = _unit.Assignments.GetById(assignmentId);
            if (assignModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any assignments"
                };
            }

            if (!TimeOnly.TryParse(time, out TimeOnly timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };
            }


            var isSuccess = await _unit.Assignments.ChangeTimeAsync(assignModel, timeOnly);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
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

        public async Task<Response> ChangeTitleAsync(long assignmentId, string title)
        {
            var assignModel = _unit.Assignments.GetById(assignmentId);
            if (assignModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any assignments"
                };
            }

            var isSuccess = await _unit.Assignments.ChangeTitleAsync(assignModel, title);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
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

        public async Task<Response> CreateAsync(AssignmentDto model)
        {
            if(!TimeOnly.TryParse(model.Time, out TimeOnly timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };
            }

            var isExistCourseContent = _unit.CourseContents.IsExist(c => c.ContentId == model.ContentId);
            if (!isExistCourseContent)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course contents"
                };
            }

            var isExistedNoNum = _unit.Assignments.IsExist(a => a.CourseContentId == model.ContentId && a.NoNum == model.NoNum);
            if (isExistedNoNum)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The current order already exists"
                };
            }

            var assignModel = _mapper.Map<Assignment>(model);
            assignModel.Time = timeOnly;

            _unit.Assignments.Add(assignModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long assignmentId)
        {
            var assignModel = _unit.Assignments.GetById(assignmentId);
            if(assignModel == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments"
                };
            }

            _unit.Assignments.Remove(assignModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<Response> GetAllAsync()
        {
            var assignments = _unit.Assignments.GetAll()
                                            .OrderBy(a => a.CourseContentId)
                                            .ThenBy(a => a.NoNum);

            foreach(var assign in assignments)
            {
                foreach(var item in assign.AssignQues)
                {
                    await _unit.AssignQues.LoadQuestionAsync(item);
                }
            }

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignmentResDto>>(assignments)
            };
        }

        public async Task<Response> GetAsync(long assignmentId)
        {
            var assignment = _unit.Assignments.GetById(assignmentId);

            foreach(var item in assignment.AssignQues)
            {
                await _unit.AssignQues.LoadQuestionAsync(item);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AssignmentResDto>(assignment),
                Success = true
            };
        }

        public async Task<Response> GetByCourseContentAsync(long courseContentId)
        {
            var assignments = await _unit.Assignments.GetByCourseContentAsync(courseContentId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignmentDto>>(assignments),
                Success = true
            };
        }

        public async Task<Response> GetNumberByCourseAsync(string courseId)
        {
            if(!_unit.Courses.IsExist(c => c.CourseId == courseId))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            var assignNum = await _unit.Assignments.GetNumberByCourseAsync(courseId);

            return new Response()
            {
                Message = assignNum,
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> GetTotalTimeByCourseAsync(string courseId)
        {
            if (!_unit.Courses.IsExist(c => c.CourseId == courseId))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            var totalTime = await _unit.Assignments.GetTotalTimeByCourseAsync(courseId);
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = totalTime,
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long assignmentId, AssignmentDto model)
        {
            var response = await _unit.Assignments.UpdateAsync(assignmentId, model);

            if (response.Success)
            {
                await _unit.CompleteAsync();
            }

            return response;
        }
    }
}
