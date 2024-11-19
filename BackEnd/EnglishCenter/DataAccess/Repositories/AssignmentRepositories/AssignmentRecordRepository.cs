using System.Text.RegularExpressions;
using AutoMapper.Configuration.Annotations;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class AssignmentRecordRepository : GenericRepository<AssignmentRecord>, IAssignmentRecordRepository
    {
        private readonly IAssignQuesRepository _assignRepo;

        public AssignmentRecordRepository(EnglishCenterContext context, IAssignQuesRepository assignRepo) : base(context)
        {
            _assignRepo = assignRepo;
        }

        public async Task<bool> ChangeAssignQuesAsync(AssignmentRecord record, long assignQueId)
        {
            if(record == null) return false;

            var assignQueModel = await context.AssignQues.FindAsync(assignQueId);
            if (assignQueModel == null) return false;

            if (record.SubQueId.HasValue)
            {
                var isExistSub = await IsExistSubAsync((QuesTypeEnum)assignQueModel.Type, record.SubQueId.Value);

                if (!isExistSub) return false;
            }

            record.AssignQuesId = assignQueId;
            record.IsCorrect = await _assignRepo.IsCorrectAnswerAsync(assignQueModel, record.SelectedAnswer, record.SubQueId);

            return true;
        }

        public async Task<bool> ChangeProcessAsync(AssignmentRecord record, long processId)
        {
            if (record == null) return false;

            var isExistProcess = await context.LearningProcesses.AnyAsync(p => p.ProcessId == processId);
            if (!isExistProcess) return false;

            record.LearningProcessId = processId;

            return true;
        }

        public async Task<bool> ChangeSelectedAnswerAsync(AssignmentRecord record, string selectedAnswer)
        {
            if (record == null) return false;

            string pattern = "^[ABCDabcd]+$";
            if (!Regex.IsMatch(selectedAnswer, pattern)) return false;

            var assignQueModel = await context.AssignQues.FindAsync(record.AssignQuesId);
            if(assignQueModel == null) return false;    

            record.SelectedAnswer = selectedAnswer.ToUpper();
            record.IsCorrect = await _assignRepo.IsCorrectAnswerAsync(assignQueModel, selectedAnswer, record.SubQueId);

            return true;
        }
        
        public async Task<bool> ChangeSubAsync(AssignmentRecord record, long? subId)
        {
            if (record == null) return false;

            var assignQueModel = await context.AssignQues.FindAsync(record.AssignQuesId);
            if(assignQueModel == null) return false;

            var isExist = await IsExistSubAsync((QuesTypeEnum)assignQueModel.Type, subId);
            if (!isExist) return false;

            record.SubQueId = subId;
            record.IsCorrect = await _assignRepo.IsCorrectAnswerAsync(assignQueModel, record.SelectedAnswer , record.SubQueId);
            return true;
        }

        public async Task<bool> IsExistSubAsync(QuesTypeEnum type, long? subId)
        {
            var isExist = false;
            switch (type)
            {
                case QuesTypeEnum.Conversation:
                    isExist = await context.SubLcConversations.AnyAsync(s => s.SubId == subId);
                    break;
                case QuesTypeEnum.Single:
                    isExist = await context.SubRcSingles.AnyAsync(s => s.SubId == subId);
                    break;
                case QuesTypeEnum.Double:
                    isExist = await context.SubRcDoubles.AnyAsync(s => s.SubId == subId);
                    break;
                case QuesTypeEnum.Triple:
                    isExist = await context.SubRcTriples.AnyAsync(s => s.SubId == subId);
                    break;
                default:
                    if (subId.HasValue) return false;
                    isExist = true;
                    break;
            }

            return isExist;
        }
    }
}
