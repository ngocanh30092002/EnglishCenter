using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IToeicExamRepository : IGenericRepository<ToeicExam>
    {
        public Task<bool> ChangeNameAsync(ToeicExam exam, string name);
        public Task<bool> ChangeCodeAsync(ToeicExam exam, int code);
        public Task<bool> ChangeYearAsync(ToeicExam exam, int year);
        public Task<bool> ChangeCompleteNumAsync(ToeicExam exam, int num);
        public Task<bool> ChangePointAsync(ToeicExam exam, int point);
        public Task<bool> ChangeMinutesAsync(ToeicExam exam, int minutes);
    }
}
