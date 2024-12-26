using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class LearningProcessRepository : GenericRepository<LearningProcess>, ILearningProcessRepository
    {
        public LearningProcessRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeStartTimeAsync(LearningProcess processModel, DateTime dateTime)
        {
            if (processModel == null) return Task.FromResult(false);

            if (processModel.EndTime.HasValue && processModel.EndTime.Value < dateTime)
            {
                return Task.FromResult(false);
            }

            processModel.StartTime = dateTime;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeEndTimeAsync(LearningProcess processModel, DateTime dateTime)
        {
            if (processModel == null) return Task.FromResult(false);

            if (processModel.StartTime > dateTime)
            {
                return Task.FromResult(false);
            }

            processModel.EndTime = dateTime;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeStatusAsync(LearningProcess processModel, ProcessStatusEnum statusEnum)
        {
            if (processModel == null) return Task.FromResult(false);

            processModel.Status = (int)statusEnum;

            return Task.FromResult(true);
        }

        public async Task<List<long>> GetExamsProcessAsync(long enrollId, string classId)
        {
            var classModel = await context.Classes.FindAsync(classId);
            if (classModel == null)
            {
                return new List<long>();
            }

            var examIds = context.Examinations
                                .Include(e => e.CourseContent)
                                .Where(e => e.CourseContent.CourseId == classModel.CourseId &&
                                            e.CourseContent.Type != 1)
                                .Select(e => e.ExamId)
                                .ToList();

            var learningProcess = context.LearningProcesses
                                         .Where(p => p.EnrollId == enrollId && p.ExamId.HasValue && examIds.Contains(p.ExamId.Value))
                                         .Select(p => p.ProcessId)
                                         .ToList();

            return learningProcess;
        }
    }
}
