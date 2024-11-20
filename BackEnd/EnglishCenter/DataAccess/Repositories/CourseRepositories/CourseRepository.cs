using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.CourseRepositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly IWebHostEnvironment _webHost;

        public CourseRepository(EnglishCenterContext context, IWebHostEnvironment webHost) : base(context)
        {
            _webHost = webHost;
        }

        public async Task<Response> UpdateAsync(string courseId, CourseDto model)
        {
            var course = await context.Courses.FindAsync(courseId);

            if (course == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The course doesn't exist yet",
                    Success = false
                };
            }

            course.Name = model.Name;
            course.Description = model.Description;
            course.EntryPoint = model.EntryPoint;
            course.StandardPoint = model.StandardPoint;

            if (model.Priority.HasValue && course.Priority != model.Priority)
            {
                var isSuccess = await this.ChangePriorityAsync(course, model.Priority.Value);

                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't change priority",
                        Success = false
                    };
                }
            }

            if (model.Image != null)
            {
                var isSuccess = await UploadImageAsync(course, model.Image);
                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false
                    };
                }
            }

            if (model.ImageThumbnail != null)
            {
                var isSuccess = await UploadImageThumbnailAsync(course, model.ImageThumbnail);
                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false
                    };
                }
            }

            return new Response()
            {
                Success = true,
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<bool> UploadImageAsync(Course courseModel, IFormFile image)
        {
            if (courseModel == null) return false;

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "courses", "images");
            var fileName = $"{DateTime.Now.Ticks}_{image.FileName}";
            var result = await UploadHelper.UploadFileAsync(image, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result)) return false;

            var wwwRootPath = _webHost.WebRootPath;
            var previousImage = Path.Combine(wwwRootPath, courseModel.Image ?? "");

            if (File.Exists(previousImage))
            {
                File.Delete(previousImage);
            }

            courseModel.Image = Path.Combine("courses", "images", fileName);
            return true;
        }

        public async Task<bool> UploadImageThumbnailAsync(Course courseModel, IFormFile image)
        {
            if (courseModel == null) return false;

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "courses", "images-thumbnail");
            var fileName = $"{DateTime.Now.Ticks}_{image.FileName}";
            var result = await UploadHelper.UploadFileAsync(image, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result)) return false;

            var wwwRootPath = _webHost.WebRootPath;
            var previousImage = Path.Combine(wwwRootPath, courseModel.ImageThumbnail ?? "");

            if (File.Exists(previousImage))
            {
                File.Delete(previousImage);
            }

            courseModel.ImageThumbnail = Path.Combine("courses", "images-thumbnail", fileName);
            return true;
        }

        public async Task<bool> ChangePriorityAsync(Course courseModel, int priority)
        {
            if (courseModel == null) return false;

            var maxNum = context.Courses.Max(c => c.Priority);
            if (priority > maxNum) return false;

            var isExist = context.Courses.Any(c => c.Priority == priority);
            if (!isExist)
            {
                courseModel.Priority = priority;
                return true;
            }

            List<Course> courses;

            if (courseModel.Priority < priority)
            {
                courses = await context.Courses.Where(c => c.Priority > courseModel.Priority)
                                                   .OrderBy(c => c.Priority)
                                                   .ToListAsync();
            }
            else if (courseModel.Priority > priority)
            {
                courses = await context.Courses.Where(c => c.Priority < courseModel.Priority)
                                                   .OrderByDescending(c => c.Priority)
                                                   .ToListAsync();
            }
            else
            {
                return true;
            }

            int currentPriority = courseModel.Priority ?? 0;

            foreach (var course in courses)
            {
                int coursePriority = course.Priority ?? 0;
                course.Priority = currentPriority;
                currentPriority = coursePriority;

                if (coursePriority == priority)
                {
                    courseModel.Priority = priority;
                    break;
                }
            }

            return true;
        }

        public async Task<Course?> GetPreviousAsync(Course courseModel)
        {
            if (courseModel == null) return null;
            if (!courseModel.Priority.HasValue) return null;

            var previousCourse = await context.Courses.FirstOrDefaultAsync(c => c.Priority == courseModel.Priority - 1);
            if (previousCourse == null) return null;

            return previousCourse;
        }
    }
}
