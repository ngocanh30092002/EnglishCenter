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

            if(processModel.StartTime > dateTime)
            {
                return Task.FromResult(false);
            }

            processModel.EndTime = dateTime;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeStatusAsync(LearningProcess processModel, ProcessStatusEnum statusEnum) 
        {
            if (processModel == null) return Task.FromResult(false);

            processModel.Status = (int) statusEnum;

            return Task.FromResult(true);
        }
    }
}
