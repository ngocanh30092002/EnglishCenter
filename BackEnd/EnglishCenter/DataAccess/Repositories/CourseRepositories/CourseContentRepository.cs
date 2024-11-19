using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.CourseRepositories
{
    public class CourseContentRepository : GenericRepository<CourseContent>, ICourseContentRepository
    {

        public CourseContentRepository(EnglishCenterContext context) : base(context)
        {

        }

        public async Task<List<CourseContent>?> GetByCourseAsync(string courseId)
        {
            if (string.IsNullOrEmpty(courseId)) return null;

            var isExistCourse = await context.Courses.AnyAsync(c => c.CourseId == courseId);
            if (!isExistCourse) return null;

            var courseContents = await context.CourseContents
                                                .Include(c => c.Assignments.OrderBy(a => a.NoNum))
                                                .Include(c => c.Examination)
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
            else if (contentModel.NoNum > number)
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
                var isSuccess = await this.ChangeCourseAsync(courseContent, model.CourseId);

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

            if (model.NoNum.HasValue && courseContent.NoNum != model.NoNum)
            {
                var isSuccess = await ChangeNoNumAsync(courseContent, model.NoNum.Value);

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

            if (model.Type!.Value != courseContent.Type)
            {
                if (!Enum.IsDefined(typeof(CourseContentTypeEnum), model.Type.Value))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Invalid type",
                        Success = false
                    };
                }

                var isSuccess = await ChangeTypeAsync(courseContent, (CourseContentTypeEnum)model.Type.Value);
                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = "Can't change type"
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

            if (newCourseContents == null || newCourseContents.Count == 0)
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

        public async Task<bool> ChangeTypeAsync(CourseContent contentModel, CourseContentTypeEnum type)
        {
            if (contentModel == null) return false;


            var assignments = await context.Assignments
                                            .Where(a => a.CourseContentId == contentModel.ContentId)
                                            .ToListAsync();

            if (assignments.Count() > 1) return false;

            contentModel.Type = (int)type;

            return true;
        }

        public async Task<CourseContent?> GetPreviousAsync(CourseContent content)
        {
            if (content == null) return null;

            if (content.NoNum == 1) return null;

            return await context.CourseContents
                                .Include(c => c.Examination)
                                .FirstOrDefaultAsync(c => c.CourseId == content.CourseId && c.NoNum == content.NoNum - 1);
        }

        public async Task<string> GetTotalTimeByCourseAsync(string courseId)
        {
            var assignments = await context.Assignments
                                           .Include(c => c.CourseContent)
                                           .Where(c => c.CourseContent.CourseId == courseId &&
                                                       c.CourseContent.Type == (int)CourseContentTypeEnum.Normal)
                                           .ToListAsync();

            var totalTime = TimeSpan.Zero;

            foreach (var assign in assignments)
            {
                totalTime = totalTime.Add(assign.Time.ToTimeSpan());
            }

            var exams = await context.Examinations
                                     .Include(c => c.CourseContent)
                                     .Where(c => c.CourseContent.CourseId == courseId &&
                                                 c.CourseContent.Type != (int)CourseContentTypeEnum.Normal)
                                     .ToListAsync();

            foreach (var exam in exams)
            {
                totalTime = totalTime.Add(exam.Time.ToTimeSpan());
            }

            return totalTime.Hours + ":" + totalTime.Minutes;
        }
    }
}
