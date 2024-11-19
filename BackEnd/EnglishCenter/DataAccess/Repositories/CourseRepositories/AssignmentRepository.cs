using AutoMapper;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.CourseRepositories
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(EnglishCenterContext context) : base(context)
        {
            
        }

        public override Assignment GetById(long id)
        {
            var model = context.Assignments.Include(a => a.AssignQues).FirstOrDefault(a => a.AssignmentId == id);
            
            return model;
        }

        public override IEnumerable<Assignment> GetAll()
        {
            var models = context.Assignments
                                .Include(a => a.AssignQues)
                                .ToList();

            return models;
        }

        public async Task<Assignment?> GetPreviousAssignmentAsync(long id)
        {
            var assignmentModel = await context.Assignments
                                               .Include(a => a.CourseContent)
                                               .FirstOrDefaultAsync(a => a.AssignmentId == id);

            if (assignmentModel == null) return null;

            if(assignmentModel.NoNum == 1 && assignmentModel.CourseContent.NoNum == 1)
            {
                return null;
            }
            else if(assignmentModel.NoNum == 1 && assignmentModel.CourseContent.NoNum != 1)
            {
                return context.Assignments
                            .Include(a => a.CourseContent)
                            .OrderByDescending(a => a.NoNum)
                            .FirstOrDefault(a => a.CourseContent.NoNum == assignmentModel.CourseContent.NoNum - 1 
                                                && a.CourseContent.CourseId == assignmentModel.CourseContent.CourseId);
            }
            else
            {
                return context.Assignments
                            .Where(a => a.NoNum == assignmentModel.NoNum - 1 && a.CourseContentId == assignmentModel.CourseContentId)
                            .FirstOrDefault();
            }
        }

        public async Task<bool> ChangeCourseContentAsync(Assignment assignmentModel, long contentId)
        {
            if (assignmentModel == null) return false;

            var courseContentModel = await context.CourseContents.Include(c => c.Assignments).FirstOrDefaultAsync(c=> c.ContentId == contentId);
            if (courseContentModel == null) return false;
            if (courseContentModel.Type != 1) return false;

            var currentAssigns = await context.Assignments
                                                        .Where(c => c.CourseContentId == assignmentModel.CourseContentId && c.NoNum > assignmentModel.NoNum)
                                                        .OrderBy(c => c.NoNum)
                                                        .ToListAsync();

            var currentNum = assignmentModel.NoNum;
            foreach(var item in currentAssigns)
            {
                int itemNum = item.NoNum;
                item.NoNum = currentNum;
                currentNum = itemNum;
            }

            assignmentModel.CourseContentId = contentId;
            
            var newCourseContentAssigns = await context.Assignments
                                                        .Where(a => a.CourseContentId == assignmentModel.CourseContentId && a.NoNum >= assignmentModel.NoNum)
                                                        .OrderBy(a => a.NoNum)
                                                        .ToListAsync();
            if(newCourseContentAssigns == null || newCourseContentAssigns.Count == 0)
            {
                var num = context.Assignments.Where(a => a.CourseContentId == assignmentModel.CourseContentId)
                                                .Count();

                if(num != assignmentModel.NoNum - 1)
                {
                    assignmentModel.NoNum = num + 1;
                }
            }
            else
            {
                int current = assignmentModel.NoNum + 1;

                foreach(var item in newCourseContentAssigns)
                {
                    item.NoNum = current;
                    current++;
                }
            }

            return true;
        }

        public Task<bool> ChangeNoNumberAsync(Assignment assignmentModel, int number)
        {
            if (assignmentModel == null) return Task.FromResult(false);

            var assignmentList = context.Assignments.Local.Concat(context.Assignments)
                                        .Where(a => a.CourseContentId == assignmentModel.CourseContentId)
                                        .Distinct()
                                        .ToList();
            if (number > assignmentList.Count)
            {
                return Task.FromResult(false);
            }

            var isExist = assignmentList.Any(a => a.CourseContentId == assignmentModel.CourseContentId && a.NoNum == number);
            if (!isExist)
            {
                assignmentModel.NoNum = number;
                return Task.FromResult(true);
            }

            List<Assignment> assignments;
            if (assignmentModel.NoNum < number)
            {
                assignments = assignmentList.Where(a => a.CourseContentId == assignmentModel.CourseContentId && a.NoNum > assignmentModel.NoNum)
                                                        .OrderBy(a => a.NoNum)
                                                        .ToList();
            }
            else if (assignmentModel.NoNum > number)
            {
                assignments = assignmentList.Where(a => a.CourseContentId == assignmentModel.CourseContentId && a.NoNum < assignmentModel.NoNum)
                                                        .OrderByDescending(a => a.NoNum)
                                                        .ToList();
            }
            else
            {
                return Task.FromResult(true);
            }

            int currentNoNum = assignmentModel.NoNum;

            foreach (var item in assignments)
            {
                int itemNoNum = item.NoNum;
                item.NoNum = currentNoNum;
                currentNoNum = itemNoNum;

                if (itemNoNum == number)
                {
                    assignmentModel.NoNum = number;
                    break;
                }
            }

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTimeAsync(Assignment assignmentModel, TimeOnly time)
        {
            if (assignmentModel == null) return Task.FromResult(false);

            assignmentModel.Time = time;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTitleAsync(Assignment assignmentModel, string title)
        {
            if (assignmentModel == null) return Task.FromResult(false);

            assignmentModel.Title = title;

            return Task.FromResult(true);
        }

        public Task<bool> ChangePercentageAsync(Assignment assignmentModel, int percentage)
        {
            if (assignmentModel == null) return Task.FromResult(false);
            if (percentage < 0 && percentage > 100) return Task.FromResult(false);

            assignmentModel.AchievedPercentage = percentage;

            return Task.FromResult(true);
        }

        public async Task<List<Assignment>> GetByCourseAsync(string courseId)
        {
            var assignments = await context.Assignments
                                        .Include(c => c.CourseContent)
                                        .Where(c => c.CourseContent.CourseId == courseId)
                                        .OrderBy(a => a.CourseContent)
                                        .ThenBy(a => a.NoNum)
                                        .ToListAsync();
            return assignments;
        }

        public async Task<List<Assignment>> GetByCourseContentAsync(long contentId)
        {
            var assignments = await context.Assignments
                                            .Where(a => a.CourseContentId == contentId)
                                            .OrderBy(a => a.NoNum)
                                            .ToListAsync();
            return assignments;
        }

        public async Task<int> GetNumberByCourseAsync(string courseId)
        {
            var assignments = await (from ct in context.CourseContents
                                    join a in context.Assignments
                                    on ct.ContentId equals a.CourseContentId
                                    where ct.CourseId == courseId
                                    select a).ToListAsync();

            return assignments.Count;

        }

        public async Task<string> GetTotalTimeByCourseAsync(string courseId)
        {
            var assignments = await (from c in context.Courses
                                    join ct in context.CourseContents
                                       on c.CourseId equals ct.CourseId
                                    join a in context.Assignments
                                       on ct.ContentId equals a.CourseContentId
                                    where c.CourseId == courseId
                                    select a).ToListAsync();

            var totalTime = TimeSpan.Zero;

            foreach (var assign in assignments)
            {
                totalTime = totalTime.Add(assign.Time.ToTimeSpan());
            }

            return totalTime.Hours + ":" + totalTime.Minutes;
        }

        public async Task<Response> UpdateAsync(long assignmentId, AssignmentDto model)
        {
            var assignment = await context.Assignments.FindAsync(assignmentId);
            if (assignment == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments"
                };
            }

            if (!string.IsNullOrEmpty(model.Time))
            {
                if (!TimeOnly.TryParse(model.Time, out TimeOnly time))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Time is not in correct format"
                    };
                }

                assignment.Time = time;
            }

            if (assignment.CourseContentId != model.ContentId)
            {
                var isSuccess = await this.ChangeCourseContentAsync(assignment, model.ContentId);

                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't change course content",
                        Success = false
                    };
                };
            }

            if (model.NoNum.HasValue && model.NoNum.Value != assignment.NoNum)
            {
                var isSuccess = await ChangeNoNumberAsync(assignment, model.NoNum.Value);

                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't change NoNum",
                        Success = false
                    };
                };
            }

            if(assignment.AchievedPercentage != model.Achieved_Percentage)
            {
                var isSuccess = await ChangePercentageAsync(assignment, model.Achieved_Percentage);

                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't change no number",
                        Success = false
                    };
                };
            }

            assignment.Title = model.Title;

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

    }
}
