using AutoMapper;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
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

        public async Task<bool> ChangeCourseContentAsync(Assignment assignmentModel, long contentId)
        {
            if (assignmentModel == null) return false;

            var isExist = context.CourseContents.Any(c => c.ContentId == contentId);
            if (!isExist) return false;

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
            var assignments = await (from c in context.Courses
                                    join ct in context.CourseContents
                                       on c.CourseId equals ct.CourseId
                                    join a in context.Assignments
                                       on ct.ContentId equals a.CourseContentId
                                    where c.CourseId == courseId
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

                //assignment.Time = time;
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

            assignment.Title = model.Title;

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        //public async Task<Response> ChangeCourseContentAsync(long assignmentId, long contentId)
        //{
        //    var assignment = await _context.Assignments.FindAsync(assignmentId);

        //    if (assignment == null)
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Can't find any assignments"
        //        };
        //    }

        //    if (assignment.CourseContentId != contentId)
        //    {
        //        var isExist = _context.CourseContents.Any(c => c.ContentId == contentId);
        //        if (!isExist)
        //        {
        //            return new Response()
        //            {
        //                StatusCode = System.Net.HttpStatusCode.BadRequest,
        //                Message = "Can't find any course contents"
        //            };
        //        }

        //        assignment.CourseContentId = contentId;
        //    }

        //    await _context.SaveChangesAsync();

        //    return new Response()
        //    {
        //        Success = true,
        //        StatusCode = System.Net.HttpStatusCode.OK,
        //        Message = ""
        //    };
        //}

        //public async Task<Response> ChangeCourseContentTitleAsync(long assignmentId, string title)
        //{
        //    var assignment = await _context.Assignments.FindAsync(assignmentId);
        //    if (assignment == null)
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Can't find any assignments"
        //        };
        //    }

        //    assignment.Title = title;
        //    await _context.SaveChangesAsync();

        //    return new Response()
        //    {
        //        Message = "",
        //        Success = true,
        //        StatusCode = System.Net.HttpStatusCode.OK
        //    };
        //}

        //public async Task<Response> ChangeNoNumberAsync(long assignmentId, int number)
        //{
        //    var assignment = await _context.Assignments.FindAsync(assignmentId);
        //    if (assignment == null)
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Can't find any assignments"
        //        };
        //    }

        //    var isExist = _context.Assignments.Any(a => a.CourseContentId == assignment.CourseContentId && a.NoNum == number);

        //    if (!isExist)
        //    {
        //        assignment.NoNum = number;
        //        await _context.SaveChangesAsync();

        //        return new Response()
        //        {
        //            Message = "",
        //            Success = true,
        //            StatusCode = System.Net.HttpStatusCode.OK,
        //        };
        //    }

        //    List<Assignment> assignments;
        //    if (assignment.NoNum < number)
        //    {
        //        assignments = await _context.Assignments.Where(a => a.CourseContentId == assignment.CourseContentId && a.NoNum > assignment.NoNum)
        //                                                .OrderBy(a => a.NoNum)
        //                                                .ToListAsync();
        //    }
        //    else
        //    {
        //        assignments = await _context.Assignments.Where(a => a.CourseContentId == assignment.CourseContentId && a.NoNum < assignment.NoNum)
        //                                                .OrderByDescending(a => a.NoNum)
        //                                                .ToListAsync();
        //    }

        //    int currentNoNum = assignment.NoNum;

        //    foreach (var item in assignments)
        //    {
        //        int itemNoNum = item.NoNum;
        //        item.NoNum = currentNoNum;
        //        currentNoNum = itemNoNum;

        //        if (itemNoNum == number)
        //        {
        //            assignment.NoNum = number;
        //            break;
        //        }
        //    }

        //    _context.Assignments.UpdateRange(assignments);
        //    _context.Assignments.Update(assignment);

        //    await _context.SaveChangesAsync();

        //    return new Response()
        //    {
        //        Message = "",
        //        Success = true,
        //        StatusCode = System.Net.HttpStatusCode.OK
        //    };
        //}

        //public async Task<Response> ChangeTimeAsync(long assignmentId, string time)
        //{
        //    var assignment = await _context.Assignments.FindAsync(assignmentId);
        //    if (assignment == null)
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Can't find any assignments"
        //        };
        //    }

        //    if (!TimeOnly.TryParse(time, out TimeOnly timeOnly))
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Time is not in correct format"
        //        };
        //    }

        //    assignment.Time = timeOnly;
        //    await _context.SaveChangesAsync();

        //    return new Response()
        //    {
        //        StatusCode = System.Net.HttpStatusCode.BadRequest,
        //        Message = "Time is not in correct format"
        //    };
        //}

        //public async Task<Response> CreateAssignmentAsync(AssignmentDto model)
        //{
        //    if (!TimeOnly.TryParse(model.Time, out TimeOnly time))
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Time is not in correct format"
        //        };
        //    }

        //    var courseContent = await _context.CourseContents.FindAsync(model.ContentId);
        //    if (courseContent == null)
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Can't find any course contents"
        //        };
        //    }

        //    var isExisted = _context.Assignments.Any(a => a.CourseContentId == model.ContentId && a.NoNum == model.NoNum);
        //    if (isExisted)
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "The current order already exists"
        //        };
        //    }

        //    var assignmentModel = _mapper.Map<Assignment>(model);
        //    assignmentModel.Time = time;

        //    _context.Assignments.Add(assignmentModel);
        //    await _context.SaveChangesAsync();

        //    return new Response()
        //    {
        //        StatusCode = System.Net.HttpStatusCode.OK,
        //        Message = "",
        //        Success = true
        //    };
        //}

        //public async Task<Response> GetAssignmentAsync(long assignmentId)
        //{
        //    var assignment = await _context.Assignments.FindAsync(assignmentId);

        //    return new Response()
        //    {
        //        Success = true,
        //        StatusCode = System.Net.HttpStatusCode.OK,
        //        Message = _mapper.Map<AssignmentDto>(assignment)
        //    };
        //}

        //public async Task<Response> GetAssignmentsAsync()
        //{
        //    var assignments = await _context.Assignments.ToListAsync();

        //    return new Response()
        //    {
        //        Success = true,
        //        StatusCode = System.Net.HttpStatusCode.OK,
        //        Message = _mapper.Map<List<AssignmentDto>>(assignments)
        //    };
        //}

        //public async Task<Response> GetAssignmentsAsync(long contentId)
        //{
        //    var assignments = await _context.Assignments
        //                                    .Where(a => a.CourseContentId == contentId)
        //                                    .OrderBy(a => a.NoNum)
        //                                    .ToListAsync();
        //    return new Response()
        //    {
        //        Success = true,
        //        StatusCode = System.Net.HttpStatusCode.OK,
        //        Message = _mapper.Map<List<AssignmentDto>>(assignments)
        //    };
        //}

        //public async Task<Response> GetNumberAssignmentsAsync(string courseId)
        //{
        //    var assignments = await (from c in _context.Courses
        //                             join ct in _context.CourseContents
        //                                on c.CourseId equals ct.CourseId
        //                             join a in _context.Assignments
        //                                on ct.ContentId equals a.CourseContentId
        //                             where c.CourseId == courseId
        //                             select a).ToListAsync();

        //    return new Response()
        //    {
        //        Success = true,
        //        StatusCode = System.Net.HttpStatusCode.OK,
        //        Message = assignments.Count
        //    };

        //}

        //public async Task<Response> GetTotalTimeAssignmentsAsync(string courseId)
        //{
        //    var assignments = await (from c in _context.Courses
        //                             join ct in _context.CourseContents
        //                                on c.CourseId equals ct.CourseId
        //                             join a in _context.Assignments
        //                                on ct.ContentId equals a.CourseContentId
        //                             where c.CourseId == courseId
        //                             select a).ToListAsync();

        //    var totalTime = TimeSpan.Zero;

        //    foreach (var assign in assignments)
        //    {
        //        if (assign.Time.HasValue)
        //        {
        //            totalTime = totalTime.Add(assign.Time.Value.ToTimeSpan());
        //        }
        //    }

        //    return new Response()
        //    {
        //        Success = true,
        //        StatusCode = System.Net.HttpStatusCode.OK,
        //        Message = totalTime.Hours + ":" + totalTime.Minutes
        //    };
        //}

        //public Task<Response> GetTotalTimeAssignmentsAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<Response> RemoveAssignmentAsync(long assignmentId)
        //{
        //    var assignment = await _context.Assignments.FindAsync(assignmentId);

        //    if (assignment == null)
        //    {
        //        return new Response()
        //        {
        //            Success = false,
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Can't find any assignments"
        //        };
        //    }

        //    _context.Assignments.Remove(assignment);
        //    await _context.SaveChangesAsync();

        //    return new Response()
        //    {
        //        Success = true,
        //        StatusCode = System.Net.HttpStatusCode.OK,
        //        Message = ""
        //    };
        //}


    }
}
