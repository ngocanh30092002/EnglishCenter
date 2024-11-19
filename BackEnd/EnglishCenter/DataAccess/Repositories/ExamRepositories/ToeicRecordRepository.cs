using System.Text.RegularExpressions;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class ToeicRecordRepository : GenericRepository<ToeicRecord>, IToeicRecordRepository
    {
        public ToeicRecordRepository(EnglishCenterContext context) : base(context)
        {
        }

        public async Task<bool> ChangeProcessAsync(ToeicRecord record, long processId)
        {
            if (record == null) return false;

            var processModel = await context.LearningProcesses.FindAsync(processId);
            if (processModel == null) return false;
            if (!processModel.ExamId.HasValue) return false;

            record.LearningProcessId = processId;

            return true;
        }

        public async Task<List<ToeicRecord>> GetResultAsync(long processId)
        {
            var records = await context.ToeicRecords
                                 .Include(r => r.SubToeic)
                                 .ThenInclude(s => s.Answer)
                                 .Where(r => r.LearningProcessId == processId)
                                 .ToListAsync();

            return records;
        }

        public async Task<int> GetNumCorrectRecordsWithPartAsync(long processId, int partNum)
        {
            var toeicRecords = await context.ToeicRecords
                                 .Where(r => r.LearningProcessId == processId)
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

        public Task<bool> ChangeSelectedAnswerAsync(ToeicRecord record, string? selectedAnswer)
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
    }
}
