using System.Text.RegularExpressions;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class ToeicPracticeRecordRepository : GenericRepository<ToeicPracticeRecord>, IToeicPracticeRecordRepository
    {
        public ToeicPracticeRecordRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeSelectedAnswerAsync(ToeicPracticeRecord record, string? selectedAnswer)
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

        public async Task<int> GetNumCorrectRecordsWithPartAsync(long attemptId, int partNum)
        {
            var toeicRecords = await context.ToeicPracticeRecords
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

        public async Task<List<ToeicPracticeRecord>> GetResultAsync(long attemptId)
        {
            var records = await context.ToeicPracticeRecords
                                 .Include(r => r.SubToeic)
                                 .ThenInclude(s => s.Answer)
                                 .Where(r => r.AttemptId == attemptId)
                                 .ToListAsync();

            return records;
        }
    }
}
