using AutoMapper;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.CourseRepositories
{
    public class CourseContentRepository : GenericRepository<CourseContent> ,ICourseContentRepository
    {
        public CourseContentRepository(EnglishCenterContext context, IMapper mapper): base(context)
        {
    
        }

        public async Task<List<CourseContent>?> GetByCourseAsync(string courseId)
        {
            if (string.IsNullOrEmpty(courseId)) return null;

            var isExistCourse = await context.Courses.AnyAsync(c => c.CourseId == courseId);
            if (!isExistCourse) return null;

            var courseContents = await context.CourseContents
                                                .Include(c => c.Assignments.OrderBy(a => a.NoNum))
                                                .OrderBy(c => c.NoNum)
                                                .Where(c => c.CourseId == courseId).ToListAsync();

            return courseContents;
        }

        public Task<bool> ChangeContentAsync(CourseContent contentModel, string content)
        {
            if (contentModel == null) return Task.FromResult(false);

            contentModel.Content = content;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeNoNumAsync(CourseContent contentModel, int number)
        {
            if (contentModel == null) return Task.FromResult(false);


            var courseContentList = context.CourseContents.Local.Concat(context.CourseContents)
                                            .Where(c => c.CourseId == contentModel.CourseId)
                                            .Distinct()
                                            .ToList();

            if (number > courseContentList.Count) return Task.FromResult(false);

            var isExist = context.CourseContents.Any(a => a.CourseId == contentModel.CourseId && a.NoNum == number);
            if (!isExist)
            {
                contentModel.NoNum = number;
                return Task.FromResult(true);
            }

            List<CourseContent> courseContents;
            if (contentModel.NoNum < number)
            {
                courseContents = courseContentList.Where(a => a.CourseId == contentModel.CourseId && a.NoNum > contentModel.NoNum)
                                                        .OrderBy(a => a.NoNum)
                                                        .ToList();
            }
            else if(contentModel.NoNum > number)
            {
                courseContents = courseContentList.Where(a => a.CourseId == contentModel.CourseId && a.NoNum < contentModel.NoNum)
                                                        .OrderByDescending(a => a.NoNum)
                                                        .ToList();
            }
            else
            {
                return Task.FromResult(true);
            }

            int currentNoNum = contentModel.NoNum;

            foreach (var item in courseContents)
            {
                int itemNoNum = item.NoNum;
                item.NoNum = currentNoNum;
                currentNoNum = itemNoNum;

                if (itemNoNum == number)
                {
                    contentModel.NoNum = number;
                    break;
                }
            }

            return Task.FromResult(true);
        }

        public async Task<Response> UpdateAsync(long contentId, CourseContentDto model)
        {
            var courseContent = await context.CourseContents.FindAsync(contentId);

            if (courseContent == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course contents"
                };
            }

            if (courseContent.CourseId != model.CourseId)
            {
                var isSuccess =  await this.ChangeCourseAsync(courseContent, model.CourseId);

                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = "Can't change CourseId"
                    };
                }
            }

            if (courseContent.NoNum != model.NoNum)
            {
                var isSuccess = await ChangeNoNumAsync(courseContent, model.NoNum);

                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = "Can't change NoNum"
                    };
                }
            }

            courseContent.Title = model.Title;
            courseContent.Content = model.Content;

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<bool> ChangeCourseAsync(CourseContent contentModel, string courseId)
        {
            if (contentModel == null) return false;

            var isExist = context.Courses.Any(c => c.CourseId == courseId);
            if (!isExist) return false;

            var courseContents = await context.CourseContents
                                                        .Where(c => c.CourseId == contentModel.CourseId && c.NoNum > contentModel.NoNum)
                                                        .OrderBy(c => c.NoNum)
                                                        .ToListAsync();

            var currentNum = contentModel.NoNum;
            foreach (var item in courseContents)
            {
                int itemNum = item.NoNum;
                item.NoNum = currentNum;
                currentNum = itemNum;
            }

            contentModel.CourseId = courseId;

            var newCourseContents = await context.CourseContents
                                                       .Where(a => a.CourseId == contentModel.CourseId && a.NoNum >= contentModel.NoNum)
                                                       .OrderBy(a => a.NoNum)
                                                       .ToListAsync();

            if(newCourseContents == null || newCourseContents.Count == 0)
            {
                var num = context.CourseContents.Where(a => a.CourseId == contentModel.CourseId)
                                                .Count();

                if (num != contentModel.NoNum - 1)
                {
                    contentModel.NoNum = num + 1;
                }
            }
            else
            {
                int current = contentModel.NoNum + 1;
                foreach (var item in newCourseContents)
                {
                    item.NoNum = current;
                    current++;
                }
            }

            return true;
        }

    }
}
