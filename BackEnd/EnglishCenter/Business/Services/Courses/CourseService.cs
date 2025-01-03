﻿using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _imageBase;
        private string _imageThumbnailBase;

        public CourseService(
            IMapper mapper,
            IUnitOfWork unit,
            IWebHostEnvironment webHostEnvironment,
            IClassService classService)
        {
            _mapper = mapper;
            _unit = unit;
            _webHostEnvironment = webHostEnvironment;
            _imageBase = Path.Combine("courses", "images");
            _imageThumbnailBase = Path.Combine("courses", "images-thumbnail");

        }
        public async Task<Response> ChangePriorityAsync(string courseId, int? priority)
        {
            var courseModel = _unit.Courses.GetById(courseId);
            if (courseModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any courses"
                };
            }

            var isSuccess = await _unit.Courses.ChangePriorityAsync(courseModel, priority);
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
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<Response> CheckIsQualifiedAsync(string userId, string courseId)
        {
            var isExistUser = _unit.Students.IsExist(s => s.UserId == userId);
            if (!isExistUser)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var courseModel = _unit.Courses.GetById(courseId);
            if (courseModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            if (!courseModel.Priority.HasValue)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = true,
                    Success = true
                };
            }

            var priority = courseModel.Priority.Value;
            var preCourseIds = _unit.Courses.Find(c => c.Priority == priority - 1).Select(c => c.CourseId).ToList();
            if (preCourseIds.Count == 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = true,
                    Success = true
                };
            }

            var highestScore = await _unit.Enrollment.GetHighestScoreAsync(userId, preCourseIds);
            if (highestScore >= courseModel.EntryPoint)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = true,
                    Success = true
                };
            }
            else
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                    Message = "You aren't eligible for admission.",
                    Success = false
                };
            }
        }

        public async Task<Response> CreateAsync(CourseDto model)
        {
            var isExistCourse = _unit.Courses.IsExist(c => c.CourseId == model.CourseId);
            if (isExistCourse)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The course already exists",
                    Success = false
                };
            }

            var courseModel = _mapper.Map<Course>(model);
            courseModel.NumLesson = 0;
            courseModel.IsSequential = model.IsSequential;

            if (!courseModel.Priority.HasValue && courseModel.EntryPoint != 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Course without priority won't have an entry point",
                    Success = false
                };
            }

            if (courseModel.Priority.HasValue && courseModel.Priority.Value == 1 && courseModel.EntryPoint != 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "First course won't have an entry point",
                    Success = false
                };
            }

            if (model.Image != null)
            {
                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, _imageBase);
                var fileName = $"{DateTime.Now.Ticks}_{model.Image?.FileName}";
                var result = await UploadHelper.UploadFileAsync(model.Image, uploadFolder, fileName);

                if (!string.IsNullOrEmpty(result))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = result
                    };
                }

                courseModel.Image = Path.Combine(_imageBase, fileName);
            }

            if (model.ImageThumbnail != null)
            {
                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, _imageThumbnailBase);
                var fileName = $"{DateTime.Now.Ticks}_{model.ImageThumbnail?.FileName}";
                var result = await UploadHelper.UploadFileAsync(model.ImageThumbnail, uploadFolder, fileName);

                if (!string.IsNullOrEmpty(result))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = result
                    };
                }

                courseModel.ImageThumbnail = Path.Combine(_imageThumbnailBase, fileName);
            }

            _unit.Courses.Add(courseModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                Success = true,
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> DeleteAsync(string courseId)
        {
            var courseModel = _unit.Courses.GetById(courseId);
            if (courseModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The course doesn't exist yet",
                    Success = false
                };
            }

            var classesCourse = _unit.Classes.Find(c => c.CourseId == courseId);

            if (classesCourse != null && classesCourse.Any(c => c.Status == (int)ClassEnum.Opening))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "There are currently classes in progress, so this course can't be deleted",
                    Success = false
                };
            }

            if (!string.IsNullOrEmpty(courseModel.Image))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, courseModel.Image);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            if (!string.IsNullOrEmpty(courseModel.ImageThumbnail))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, courseModel.ImageThumbnail);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            var isChangeSuccess = await _unit.Courses.ChangePriorityAsync(courseModel, null);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't delete course",
                    Success = false
                };
            }

            _unit.Courses.Remove(courseModel);
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
            var courses = _unit.Courses.GetAll().OrderBy(c => c.Priority);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<CourseDto>>(courses),
                Success = true
            });
        }

        public Task<Response> GetAsync(string courseId)
        {
            var course = _unit.Courses.GetById(courseId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<CourseDto>(course),
                Success = true
            });
        }

        public async Task<Response> GetPreviousAsync(string courseId)
        {
            var courseModel = _unit.Courses.GetById(courseId);
            if (courseModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The course doesn't exist yet",
                    Success = false
                };
            }

            var previousCourse = await _unit.Courses.GetPreviousAsync(courseModel);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<CourseDto>(previousCourse),
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(string courseId, CourseDto model)
        {
            var response = await _unit.Courses.UpdateAsync(courseId, model);

            if (response.Success)
            {
                await _unit.CompleteAsync();
            }

            return response;
        }

        public async Task<Response> UploadImageAsync(string courseId, IFormFile file)
        {
            var courseModel = _unit.Courses.GetById(courseId);

            if (courseModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any courses"
                };
            }

            var isSuccess = await _unit.Courses.UploadImageAsync(courseModel, file);
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
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<Response> UploadImageThumbnailAsync(string courseId, IFormFile file)
        {
            var courseModel = _unit.Courses.GetById(courseId);

            if (courseModel == null)
            {
                return new Response
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any courses"
                };
            }

            var isSuccess = await _unit.Courses.UploadImageThumbnailAsync(courseModel, file);

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
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }
    }
}
