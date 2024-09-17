using AutoMapper;
using EnglishCenter.Database;
using EnglishCenter.Helpers;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly EnglishCenterContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;

        public CourseRepository(EnglishCenterContext context, IMapper mapper, IWebHostEnvironment webHost) 
        {
            _context = context;
            _mapper = mapper;
            _webHost = webHost;
        }

        public async Task<Response> GetCoursesAsync()
        {
            var courses = await _context.Courses.OrderBy(c => c.Priority).ToListAsync();

            var courseDtoModels = _mapper.Map<List<CourseDto>>(courses);

            return new Response
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = courseDtoModels
            };
        }

        public async Task<Response> GetCourseAsync(string courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses"
                };
            }

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<CourseDto>(course)
            };
        }

        public async Task<Response> ChangePriorityAsync(string courseId, int priority)
        {
            var isExist = _context.Courses.Any(c => c.Priority == priority);

            if (!isExist)
            {
                var course = await _context.Courses.FindAsync(courseId);
                course.Priority = priority;

                await _context.SaveChangesAsync();

                return new Response()
                {
                    Message = "",
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
                };
            }

            var currentCourse = await _context.Courses.FindAsync(courseId);
            if(currentCourse == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The course doesn't exist yet",
                    Success = false
                };
            }

            List<Course> courses = null;

            if (currentCourse.Priority < priority)
            {
                courses = await _context.Courses.Where(c => c.Priority > currentCourse.Priority)
                                                   .OrderBy(c => c.Priority)
                                                   .ToListAsync();
            }
            else
            {
                courses = await _context.Courses.Where(c => c.Priority < currentCourse.Priority)
                                                   .OrderByDescending(c => c.Priority)
                                                   .ToListAsync();
            }

            int currentPriority = currentCourse.Priority ?? 0;

            foreach(var course in courses)
            {
                int coursePriority = course.Priority ?? 0;
                course.Priority = currentPriority;
                currentPriority = coursePriority;

                if(coursePriority == priority)
                {
                    currentCourse.Priority = priority;
                    break;
                }
            }

            _context.Courses.UpdateRange(courses);
            _context.Courses.Update(currentCourse);

            await _context.SaveChangesAsync();

            return new Response()
            {
                Message = "",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> CreateCourseAsync(CourseDto model)
        {
            var course = await _context.Courses.FindAsync(model.CourseId);

            if (course != null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The course already exists",
                    Success = false
                };
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "courses", "images");
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

            var courseModel = _mapper.Map<Course>(model);
            courseModel.Image = Path.Combine("courses", "images", fileName);

            _context.Courses.Add(courseModel);
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> DeleteCourseAsync(string courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if(course == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The course doesn't exist yet",
                    Success = false
                };
            }

            var classesCourse = await _context.Classes
                                            .Where(c => c.CourseId == courseId)
                                            .ToListAsync();
            if(classesCourse != null && classesCourse.Any())
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "There are currently classes in progress, so the subject can't be deleted",
                    Success = false,
                };
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
            };
        }

        public async Task<Response> UpdateCourseAsync(string courseId, CourseDto model)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if(course == null)
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
            course.NumLesson = model.NumLesson;
            course.EntryPoint = model.EntryPoint;
            course.StandardPoint = model.StandardPoint;

            _context.Courses.Update(course);

            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> UploadCourseImageAsync(string courseId, IFormFile image)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The course doesn't exist yet",
                    Success = false
                };
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "courses", "images");
            var fileName = $"{DateTime.Now.Ticks}_{image.FileName}";
            var result = await UploadHelper.UploadFileAsync(image, uploadFolder, fileName);

            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = result
                };
            }

            var wwwRootPath = _webHost.WebRootPath;
            var previousImage = Path.Combine(wwwRootPath, course.Image);

            if(File.Exists(previousImage))
            {
                File.Delete(previousImage);
            }

            course.Image = Path.Combine("courses", "images", fileName);

            await _context.SaveChangesAsync();
            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }
    }
}
