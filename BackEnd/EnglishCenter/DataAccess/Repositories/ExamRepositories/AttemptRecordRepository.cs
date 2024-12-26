using System.Text.RegularExpressions;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class AttemptRecordRepository : GenericRepository<AttemptRecord>, IAttemptRecordRepository
    {
        public AttemptRecordRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeSelectedAnswerAsync(AttemptRecord record, string? selectedAnswer)
        {
            if (record == null) return Task.FromResult(false);

            if (!string.IsNullOrEmpty(selectedAnswer))
            {
                string pattern = "^[ABCDabcd]+$";
                if (!Regex.IsMatch(selectedAnswer, pattern)) return Task.FromResult(false);
            }

            var answerModel = context.AnswerToeic.FirstOrDefault(a => a.AnswerId == record.SubToeic.AnswerId);
            if (answerModel == null) return Task.FromResult(false);

            record.SelectedAnswer = selectedAnswer;
            record.IsCorrect = answerModel.CorrectAnswer == selectedAnswer;

            return Task.FromResult(true);
        }

        public async Task<List<QuesToeic>?> GetByRoadMapQuestionAsync(long attemptId)
        {
            var subQueIds = context.AttemptRecords
                                   .Where(a => a.AttemptId == attemptId)
                                   .OrderBy(a => a.RecordId)
                                   .Select(a => a.SubQueId)
                                   .ToList();

            var quesToeicModels = new List<QuesToeic>();

            foreach (var subId in subQueIds)
            {
                var quesModel = await context.QuesToeic
                                    .Include(q => q.SubToeicList)
                                    .FirstOrDefaultAsync(q => q.SubToeicList.Any(sq => sq.SubId == subId));

                if (quesModel != null)
                {
                    quesToeicModels.Add(quesModel);
                }

            }

            return quesToeicModels.Distinct().ToList();
        }

        public async Task<int> GetNumCorrectRecordsWithPartAsync(long attemptId, int partNum)
        {
            var toeicRecords = await context.AttemptRecords
                                 .Where(r => r.AttemptId == attemptId)
                                 .ToListAsync();

            var result = (from r in toeicRecords
                          join s in context.SubToeic
                          on r.SubQueId equals s.SubId
                          join q in context.QuesToeic
                          on s.QuesId equals q.QuesId
                          where q.Part == partNum && r.IsCorrect == true
                          select r).Count();

            return result;
        }

        public async Task<List<AttemptRecord>> GetResultAsync(long attemptId)
        {
            var records = await context.AttemptRecords
                                 .Include(r => r.SubToeic)
                                 .ThenInclude(s => s.Answer)
                                 .Where(r => r.AttemptId == attemptId)
                                 .ToListAsync();

            return records;
        }
    }
}
