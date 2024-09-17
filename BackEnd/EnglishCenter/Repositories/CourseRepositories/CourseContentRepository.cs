using AutoMapper;
using EnglishCenter.Database;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Models.IdentityModel;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Repositories.CourseRepositories
{
    public class CourseContentRepository : ICourseContentRepository
    {
        private readonly EnglishCenterContext _context;
        private readonly IMapper _mapper;

        public CourseContentRepository(EnglishCenterContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> ChangeContentAsync(long contentId, string content)
        {
            var courseContent = await _context.CourseContents.FindAsync(contentId);
            if (courseContent == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course content"
                };
            }

            courseContent.Content = content;
            await _context.SaveChangesAsync();

            return new Response()
            {
                Message = "",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> ChangeNoNumAsync(long contentId, int number)
        {
            var courseContent = await _context.CourseContents.FindAsync(contentId);
            if (courseContent == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course content"
                };
            }

            var isExist = _context.CourseContents.Any(a => a.CourseId == courseContent.CourseId && a.NoNum == number);

            if (!isExist)
            {
                courseContent.NoNum = number;
                await _context.SaveChangesAsync();

                return new Response()
                {
                    Message = "",
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
                };
            }

            List<CourseContent> courseContents;
            if (courseContent.NoNum < number)
            {
                courseContents = await _context.CourseContents.Where(a => a.CourseId == courseContent.CourseId && a.NoNum > courseContent.NoNum)
                                                        .OrderBy(a => a.NoNum)
                                                        .ToListAsync();
            }
            else
            {
                courseContents = await _context.CourseContents.Where(a => a.CourseId == courseContent.CourseId && a.NoNum < courseContent.NoNum)
                                                        .OrderByDescending(a => a.NoNum)
                                                        .ToListAsync();
            }

            int currentNoNum = courseContent.NoNum;

            foreach (var item in courseContents)
            {
                int itemNoNum = item.NoNum;
                item.NoNum = currentNoNum;
                currentNoNum = itemNoNum;

                if (itemNoNum == number)
                {
                    courseContent.NoNum = number;
                    break;
                }
            }

            _context.CourseContents.UpdateRange(courseContents);
            _context.CourseContents.Update(courseContent);

            await _context.SaveChangesAsync();

            return new Response()
            {
                Message = "",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> CreateContentAsync(CourseContentDto model)
        {
            var course = await _context.Courses.FindAsync(model.CourseId);

            if(course == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any courses"
                };
            }

            var isExist = _context.CourseContents.Any(c => c.CourseId == model.CourseId && c.NoNum == model.NoNum);

            if (isExist)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The current order already exists"
                };
            }

            var courseContent = _mapper.Map<CourseContent>(model);

            _context.CourseContents.Add(courseContent);
            await _context.SaveChangesAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> GetContentAsync(long contentId)
        {
            var courseContent = await _context.CourseContents.FindAsync();

            return new Response()
            {
                Success = true,
                Message = _mapper.Map<CourseContentDto>(courseContent),
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> GetContentsAsync()
        {
            var courseContents = await _context.CourseContents.ToListAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<CourseContentDto>>(courseContents)
            };
        }

        public async Task<Response> GetContentsAsync(string courseId)
        {
            var courseContents = await _context.CourseContents
                                                .Include(c => c.Assignments.OrderBy(a => a.NoNum))
                                                .OrderBy(c => c.NoNum)
                                                .Where(c => c.CourseId == courseId).ToListAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<CourseContentDto>>(courseContents)
            };
        }

        public async Task<Response> RemoveContentAsync(long contentId)
        {
            var courseContent = await _context.CourseContents.FindAsync(contentId);

            if (courseContent == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course contents"
                };
            }

            _context.CourseContents.Remove(courseContent);
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<Response> UpdateContentAsync(long contentId, CourseContentDto model)
        {
            var courseContent = await _context.CourseContents.FindAsync(contentId);

            if (courseContent == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course contents"
                };
            }

            if(courseContent.CourseId != model.CourseId)
            {
                var course = await _context.Courses.FindAsync(model.CourseId);

                if(course == null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any courses"
                    };
                }
            }

            if(courseContent.NoNum !=  model.NoNum)
            {
                var changeRes = await ChangeNoNumAsync(courseContent.ContentId, model.NoNum);

                if (!changeRes.Success)
                {
                    return changeRes;
                }
            }

            courseContent.Title = model.Title;
            courseContent.Content = model.Content;
            courseContent.CourseId = model.CourseId;

            _context.CourseContents.Update(courseContent);
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
