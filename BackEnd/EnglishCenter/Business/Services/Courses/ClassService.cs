using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EnglishCenter.Business.Services.Courses
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClassService(IMapper mapper, IUnitOfWork unit, IWebHostEnvironment webHostEnvironment)
        {
            _unit = unit;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Response> ChangeCourseAsync(string classId, string courseId)
        {
            var isExistCourse = _unit.Courses.IsExist(c => c.CourseId == courseId);
            if(!isExistCourse)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            var classModel = _unit.Classes.GetById(classId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any classes"
                };
            }

            var isSuccess = await _unit.Classes.ChangeCourseAsync(classModel, courseId);
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

        public async Task<Response> ChangeEndTimeAsync(string classId, DateOnly endTime)
        {
            var classModel = _unit.Classes.GetById(classId);

            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any classes"
                };
            }

            if (classModel.StartDate.HasValue)
            {
                if (classModel.StartDate.Value > endTime)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = "EndDate must be greater than StartDate"
                    };
                }
            }

            var isSuccess = await _unit.Classes.ChangeEndTimeAsync(classModel, endTime);
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

        public async Task<Response> ChangeMaxNumAsync(string classId, int maxNum)
        {
            var classModel = _unit.Classes.GetById(classId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any classes"
                };
            }
            
            if(maxNum < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "MaxNum must be greater than 0"
                };
            }

            var isSuccess = await _unit.Classes.ChangeMaxNumAsync(classModel, maxNum);

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

        public async Task<Response> ChangeImageAsync(string classId, IFormFile image)
        {
            var classModel = _unit.Classes.GetById(classId);
            if(classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any classes"
                };
            }

            var isSuccess = await _unit.Classes.ChangeImageAsync(classModel, image);

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

        public async Task<Response> ChangeStartTimeAsync(string classId, DateOnly startTime)
        {
            var classModel = _unit.Classes.GetById(classId);
            
            if(classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any classes"
                };
            }

            if (classModel.EndDate.HasValue)
            {
                if(classModel.EndDate.Value < startTime)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = "StartDate must be less than EndDate"
                    };
                }
            }

            var isSuccess = await _unit.Classes.ChangeStartTimeAsync(classModel, startTime);
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

        public async Task<Response> CreateAsync(ClassDto model)
        {
            var isExistCourse = _unit.Courses.IsExist(c => c.CourseId == model.CourseId);
            if(!isExistCourse)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            var isExistClass = _unit.Classes.IsExist(c => c.ClassId == model.ClassId);
            if (isExistClass)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Class already exists",
                };
            }

            // Todo: Check Teacher is Exist ?
            var isExistTeacher = _unit.Students.IsExist(s => s.UserId == model.TeacherId);
            if (!isExistTeacher)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any teachers"
                };
            }

            if (model.StartDate.HasValue && model.EndDate.HasValue)
            {
                if (model.StartDate.Value > model.EndDate.Value)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Start time must be less than end time"
                    };
                }
            }

            var classModel = _mapper.Map<Class>(model);
            if(model.Image != null)
            {
                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "classes", "images");
                var fileName = $"{DateTime.Now.Ticks}_{model.Image.FileName}";
                var result = await UploadHelper.UploadFileAsync(model.Image, uploadFolder, fileName);

                if (!string.IsNullOrEmpty(result))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = result
                    };
                }

                classModel.Image = Path.Combine("classes", "images", fileName);
            }

            _unit.Classes.Add(classModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(string classId)
        {
            var classModel = _unit.Classes.GetById(classId);
            if(classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            _unit.Classes.Remove(classModel);
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
            var classes = _unit.Classes.GetAll();

            var response = new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ClassDto>>(classes),
                Success = true
            };

            return Task.FromResult(response);
        }

        public Task<Response> GetAsync(string classId)
        {
            var classModel = _unit.Classes.GetById(classId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ClassDto>(classModel),
                Success = true
            });
        }

        public async Task<Response> GetClassesWithTeacherAsync(string userId)
        {
            // Todo: Check Teacher is isExist ?

            var classes = await _unit.Classes.GetClassesWithTeacherAsync(userId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ClassDto>>(classes),
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(string classId, ClassDto model)
        {
            var response = await _unit.Classes.UpdateAsync(classId, model);

            if (response.Success)
            {
                await _unit.CompleteAsync();
            }

            return response;
        }

        public Task<bool> IsClassOfTeacherAsync(string userId, string classId)
        {
            var isValid = _unit.Classes.IsExist(c => c.ClassId == classId && c.TeacherId == userId);

            return Task.FromResult(isValid);
        }
    }
}
