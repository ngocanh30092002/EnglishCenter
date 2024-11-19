using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IExaminationRepository : IGenericRepository<Examination>
    {
        public Task<bool> ChangeCourseContentAsync(Examination examination, long contentId);
        public Task<bool> ChangeToeicAsync(Examination examination, long toeicId);
        public Task<bool> ChangeTitleAsync(Examination examination, string title);
        public Task<bool> ChangeTimeAsync(Examination examination, TimeOnly time);
        public Task<bool> ChangeDescriptionAsync(Examination examination, string description);
    }
}
