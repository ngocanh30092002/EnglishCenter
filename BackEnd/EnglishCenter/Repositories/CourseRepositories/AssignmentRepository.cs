using AutoMapper;
using EnglishCenter.Database;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Repositories.CourseRepositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly EnglishCenterContext _context;
        private readonly IMapper _mapper;

        public AssignmentRepository(EnglishCenterContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Response> ChangeCourseContentAsync(long assignmentId, long contentId)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);

            if (assignment == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments"
                };
            }

            if (assignment.CourseContentId != contentId)
            {
                var isExist = _context.CourseContents.Any(c => c.ContentId == contentId);
                if (!isExist)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any course contents"
                    };
                }

                assignment.CourseContentId = contentId;
            }

            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<Response> ChangeCourseContentTitleAsync(long assignmentId, string title)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);
            if (assignment == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments"
                };
            }

            assignment.Title = title;
            await _context.SaveChangesAsync();

            return new Response()
            {
                Message = "",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> ChangeNoNumberAsync(long assignmentId, int number)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);
            if(assignment == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments"
                };
            }

            var isExist = _context.Assignments.Any(a => a.CourseContentId == assignment.CourseContentId && a.NoNum == number);

            if (!isExist)
            {
                assignment.NoNum = number;
                await _context.SaveChangesAsync();

                return new Response()
                {
                    Message = "",
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
                };
            }

            List<Assignment> assignments;
            if(assignment.NoNum < number)
            {
                assignments = await _context.Assignments.Where(a => a.CourseContentId == assignment.CourseContentId && a.NoNum > assignment.NoNum)
                                                        .OrderBy(a => a.NoNum)
                                                        .ToListAsync();
            }
            else
            {
                assignments = await _context.Assignments.Where(a => a.CourseContentId == assignment.CourseContentId && a.NoNum < assignment.NoNum)
                                                        .OrderByDescending(a => a.NoNum)
                                                        .ToListAsync();
            }

            int currentNoNum = assignment.NoNum;

            foreach (var item in assignments)
            {
                int itemNoNum = item.NoNum;
                item.NoNum = currentNoNum;
                currentNoNum = itemNoNum;

                if (itemNoNum == number)
                {
                    assignment.NoNum = number;
                    break;
                }
            }

            _context.Assignments.UpdateRange(assignments);
            _context.Assignments.Update(assignment);

            await _context.SaveChangesAsync();

            return new Response()
            {
                Message = "",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> ChangeTimeAsync(long assignmentId, string time)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);
            if (assignment == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments"
                };
            }

            if(!TimeOnly.TryParse(time, out TimeOnly timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };
            }

            assignment.Time = timeOnly;
            await _context.SaveChangesAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = "Time is not in correct format"
            };
        }

        public async Task<Response> CreateAssignmentAsync(AssignmentDto model)
        {
            if(!TimeOnly.TryParse(model.Time, out TimeOnly time))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };
            }

            var courseContent = await _context.CourseContents.FindAsync(model.ContentId);
            if (courseContent == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any course contents"
                };
            }

            var isExisted = _context.Assignments.Any(a => a.CourseContentId == model.ContentId && a.NoNum == model.NoNum);
            if (isExisted)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The current order already exists"
                };
            }

            var assignmentModel = _mapper.Map<Assignment>(model);
            assignmentModel.Time = time;

            _context.Assignments.Add(assignmentModel);
            await _context.SaveChangesAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> GetAssignmentAsync(long assignmentId)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AssignmentDto>(assignment)
            };
        }

        public async Task<Response> GetAssignmentsAsync()
        {
            var assignments = await _context.Assignments.ToListAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignmentDto>>(assignments)
            };
        }

        public async Task<Response> GetAssignmentsAsync(long contentId)
        {
            var assignments = await _context.Assignments
                                            .Where(a => a.CourseContentId == contentId)
                                            .OrderBy(a => a.NoNum)
                                            .ToListAsync();
            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignmentDto>>(assignments)
            };
        }

        public async Task<Response> GetNumberAssignmentsAsync(string courseId)
        {
            var assignments = await (from c in _context.Courses
                              join ct in _context.CourseContents
                                 on c.CourseId equals ct.CourseId
                              join a in _context.Assignments
                                 on ct.ContentId equals a.CourseContentId
                              where c.CourseId == courseId
                              select a).ToListAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = assignments.Count
            };

        }

        public async Task<Response> GetTotalTimeAssignmentsAsync(string courseId)
        {
            var assignments = await(from c in _context.Courses
                                    join ct in _context.CourseContents
                                       on c.CourseId equals ct.CourseId
                                    join a in _context.Assignments
                                       on ct.ContentId equals a.CourseContentId
                                    where c.CourseId == courseId
                                    select a).ToListAsync();

            var totalTime = TimeSpan.Zero;

            foreach(var assign in assignments)
            {
                if (assign.Time.HasValue)
                {
                    totalTime = totalTime.Add(assign.Time.Value.ToTimeSpan());
                }
            }

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = totalTime.Hours + ":" + totalTime.Minutes
            };
        }

        public Task<Response> GetTotalTimeAssignmentsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Response> RemoveAssignmentAsync(long assignmentId)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);

            if(assignment == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments"
                };
            }

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<Response> UpdateAssignmentAsync(long assignmentId, AssignmentDto model)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);

            if(assignment == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments"
                };
            }

            if(model.NoNum.HasValue && model.NoNum.Value != assignment.NoNum)
            {
                await ChangeNoNumberAsync(assignmentId, model.NoNum.Value);
            }

            if (!string.IsNullOrEmpty(model.Time))
            {
                if(!TimeOnly.TryParse(model.Time, out TimeOnly time))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Time is not in correct format"
                    };
                }

                assignment.Time = time;
            }

            if(assignment.CourseContentId != model.ContentId)
            {
                var isExist = _context.CourseContents.Any(c => c.ContentId == model.ContentId);
                if (!isExist)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any course contents"
                    };
                }

                assignment.CourseContentId = model.ContentId;
            }

            assignment.Title = model.Title;
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
