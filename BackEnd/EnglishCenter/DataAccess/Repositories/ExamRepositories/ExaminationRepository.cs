using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class ExaminationRepository : GenericRepository<Examination>, IExaminationRepository
    {
        public ExaminationRepository(EnglishCenterContext context) : base(context)
        {

        }

        public async Task<bool> ChangeCourseContentAsync(Examination examination, long contentId)
        {
            if (examination == null) return false;

            var courseContent = await context.CourseContents
                                            .FirstOrDefaultAsync(c => c.ContentId == contentId && c.Type != (int)CourseContentTypeEnum.Normal);
            if (courseContent == null) return false;

            examination.ContentId = contentId;
            return true;
        }

        public Task<bool> ChangeToeicAsync(Examination examination, long toeicId)
        {
            if (examination == null) return Task.FromResult(false);

            var isExistToeic = context.ToeicExams.Any(t => t.ToeicId == toeicId);
            if (!isExistToeic) return Task.FromResult(false);

            examination.ToeicId = toeicId;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeTitleAsync(Examination examination, string title)
        {
            if (examination == null) return Task.FromResult(false);

            examination.Title = title;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTimeAsync(Examination examination, TimeOnly time)
        {
            if (examination == null) return Task.FromResult(false);

            examination.Time = time;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeDescriptionAsync(Examination examination, string description)
        {
            if (examination == null) return Task.FromResult(false);

            examination.Description = description;

            return Task.FromResult(true);
        }
    }
}
