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

            if (!model.Priority.HasValue && model.EntryPoint != 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Course with priority won't have an entry point",
                    Success = false
                };
            }

            if (model.Priority.HasValue && model.Priority.Value == 1 && model.EntryPoint != 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "First course won't have an entry point",
                    Success = false
                };
            }

            course.Name = model.Name;
            course.Description = model.Description;
            course.EntryPoint = model.EntryPoint;
            course.StandardPoint = model.StandardPoint;
            course.IsSequential = model.IsSequential;

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
            if (!model.Priority.HasValue)
            {
                var isSuccess = await this.ChangePriorityAsync(course, null);

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

        public async Task<bool> ChangePriorityAsync(Course courseModel, int? priority)
        {
            if (courseModel == null) return false;

            var maxNum = context.Courses.Max(c => c.Priority);

            if (priority.HasValue && priority > maxNum + 1) return false;

            courseModel.Priority = priority;

            var otherCourses = new List<Course>();

            if (priority.HasValue)
            {
                otherCourses = context.Courses
                                          .Where(c => c.CourseId != courseModel.CourseId && c.Priority.HasValue)
                                          .OrderBy(c => c.Priority)
                                          .ToList();
                otherCourses.Add(courseModel);
            }
            else
            {
                otherCourses = context.Courses
                                          .Where(c => c.Priority.HasValue && c.CourseId != courseModel.CourseId)
                                          .OrderBy(c => c.Priority)
                                          .ToList();
            }

            var sortedCourses = otherCourses.OrderBy(c => c.Priority).ToList();

            int currentPriority = 1;
            for (int i = 0; i < sortedCourses.Count; i++)
            {
                if (i > 0 && sortedCourses[i].Priority != sortedCourses[i - 1].Priority)
                {
                    currentPriority++;
                }

                sortedCourses[i].Priority = currentPriority;
            }

            return true;

        }

        public async Task<List<Course>?> GetPreviousAsync(Course courseModel)
        {
            if (courseModel == null) return null;
            if (!courseModel.Priority.HasValue) return null;

            var previousCourses = await context.Courses.Where(c => c.Priority == courseModel.Priority - 1).ToListAsync();

            return previousCourses;
        }
    }
}
