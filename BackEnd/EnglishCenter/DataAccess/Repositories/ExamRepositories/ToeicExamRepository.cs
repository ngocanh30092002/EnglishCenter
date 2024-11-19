using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class ToeicExamRepository : GenericRepository<ToeicExam>, IToeicExamRepository
    {
        public ToeicExamRepository(EnglishCenterContext context) : base(context)
        {

        }

        public Task<bool> ChangeCodeAsync(ToeicExam exam, int code)
        {
            if (exam == null) return Task.FromResult(false);
            if (code < 0) return Task.FromResult(false);

            exam.Code = code;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeCompleteNumAsync(ToeicExam exam, int num)
        {
            if (exam == null) return Task.FromResult(false);

            exam.CompletedNum = num;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeMinutesAsync(ToeicExam exam, int minutes)
        {
            if (exam == null) return Task.FromResult(false);

            exam.TimeMinutes = minutes;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeNameAsync(ToeicExam exam, string name)
        {
            if (exam == null) return Task.FromResult(false);

            exam.Name = name;

            return Task.FromResult(true);
        }

        public Task<bool> ChangePointAsync(ToeicExam exam, int point)
        {
            if (exam == null) return Task.FromResult(false);

            exam.Point = point;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeYearAsync(ToeicExam exam, int year)
        {
            if (exam == null) return Task.FromResult(false);
            if (year <= 2000 || year >= 3000) return Task.FromResult(false);

            exam.Year = year;

            return Task.FromResult(true);
        }
    }
}
