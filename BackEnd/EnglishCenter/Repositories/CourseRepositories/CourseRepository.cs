using EnglishCenter.Database;
using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly EnglishCenterContext _context;

        public CourseRepository(EnglishCenterContext context) 
        {
            _context = context;
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            var courses = await _context.Courses.ToListAsync();

            return courses;
        }

        public async Task<Course> GetCourseAsync(string courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            return course;
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

        public async Task<Response> CreateCourseAsync(Course model)
        {
            var course = await _context.Courses.FindAsync(model.CourseId);

            var coursePriority = await _context.Courses.FirstOrDefaultAsync(c => c.Priority == model.Priority);

            if (course != null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The course already exists",
                    Success = false
                };
            }

            if(coursePriority != null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "A course with the same priority already exists",
                    Success = false
                };
            }

            _context.Courses.Add(model);
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
            if(classesCourse != null || classesCourse.Any())
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
                Message = "",
            };
        }

        public async Task<Response> UpdateCourseAsync(string courseId, Course model)
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
    }
}
