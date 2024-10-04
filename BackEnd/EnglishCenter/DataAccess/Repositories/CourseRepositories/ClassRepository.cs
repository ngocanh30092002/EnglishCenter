using AutoMapper;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.CourseRepositories
{
    public class ClassRepository : GenericRepository<Class> ,IClassRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClassRepository(EnglishCenterContext context,  IWebHostEnvironment webHostEnvironment): base(context)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Class>?> GetClassesWithTeacherAsync(string teacherId)
        {
            if (teacherId == null) return null;

            var classes = await context.Classes
                                    .Include(c => c.Teacher)
                                    .Where(c => c.TeacherId == teacherId)
                                    .ToListAsync();
            return classes;
        }

        public async Task<bool> ChangeCourseAsync(Class model, string courseId)
        {
            if(model == null) return false;

            var course = await context.Courses.FindAsync(courseId);
            if (course == null) return false;

            model.CourseId = courseId;

            return true;
        }

        public Task<bool> ChangeEndTimeAsync(Class model, DateOnly endTime)
        {
            if (model == null) return Task.FromResult(false);

            if (model.StartDate.HasValue)
            {
                if (model.StartDate.Value > endTime)
                {
                    return Task.FromResult(false);
                }
            }

            model.EndDate = endTime;
            return Task.FromResult(true);
        }

        public async Task<bool> ChangeImageAsync(Class model, IFormFile image)
        {
            if (model == null) return false;

            var oldPathImage = Path.Combine(_webHostEnvironment.WebRootPath, model.Image ?? "");
            if (File.Exists(oldPathImage))
            {
                File.Delete(oldPathImage);
            }

            var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "classes", "images");
            var fileName = $"{DateTime.Now.Ticks}_{image.FileName}";
            var result = await UploadHelper.UploadFileAsync(image, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result)) return false;

            model.Image = Path.Combine("classes", "images", fileName);

            return true;
        }

        public Task<bool> ChangeMaxNumAsync(Class model, int maxNum)
        {
            if (model == null) return Task.FromResult(false);

            if (maxNum < 0)
            {
                return Task.FromResult(false);
            }

            model.MaxNum = maxNum;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeStartTimeAsync(Class model, DateOnly startTime)
        {
            if (model == null) return Task.FromResult(false);

            if (model.EndDate.HasValue)
            {
                if (model.EndDate.Value < startTime)
                {
                    return Task.FromResult(false);
                }
            }

            model.StartDate = startTime;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeDescriptionAsync(Class model, string newDes)
        {
            if (model == null) return Task.FromResult(false);

            model.Description = newDes;
            return Task.FromResult(true);
        }

        public async Task<Response> UpdateAsync(string classId, ClassDto model)
        {
            var classModel = await context.Classes.FindAsync(classId);

            if (classModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any classes",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };
            }

            if (classModel.CourseId != model.CourseId)
            {
                var isExistCourse = context.Courses.Any(c => c.CourseId == model.CourseId);
                if (!isExistCourse)
                {
                    return new Response()
                    {
                        Message = "Can't find any courses",
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                    };
                }

                classModel.CourseId = model.CourseId;
            }

            if (classModel.TeacherId != model.TeacherId)
            {
                var isExistTeacher = await context.Teachers.AnyAsync(u => u.UserId == model.TeacherId);
                if (!isExistTeacher)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any teachers",
                        Success = false
                    };
                }

                classModel.TeacherId = model.TeacherId;
            }

            if (model.Image != null)
            {
                var isChangeSuccess = await ChangeImageAsync(classModel, model.Image);

                if (!isChangeSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Change image failed",
                        Success = false
                    };
                }
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

            classModel.StartDate = model.StartDate;
            classModel.EndDate = model.EndDate;
            classModel.RegisteredNum = model.RegisteredNum ?? 0;
            classModel.MaxNum = model.MaxNum ?? 0;
            classModel.Description = model.Description ?? classModel.Description;

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }
    }
}
