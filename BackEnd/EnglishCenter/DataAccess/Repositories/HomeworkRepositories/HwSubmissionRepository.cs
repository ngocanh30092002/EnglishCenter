using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.DataAccess.Repositories.HomeworkRepositories
{
    public class HwSubmissionRepository : GenericRepository<HwSubmission>, IHwSubmissionRepository
    {
        public HwSubmissionRepository(EnglishCenterContext context) : base(context)
        {

        }
        public async Task<bool> ChangeDateAsync(HwSubmission submitModel, DateTime dateTime)
        {
            if (submitModel == null) return false;

            var homeworkModel = await context.Homework.FindAsync(submitModel.HomeworkId);
            if (homeworkModel == null) return false;

            if (homeworkModel.StartTime <= dateTime && dateTime <= homeworkModel.EndTime)
            {
                submitModel.SubmitStatus = (int)SubmitStatusEnum.OnTime;
            }
            else if (homeworkModel.EndTime < dateTime && dateTime <= homeworkModel.EndTime.AddDays(homeworkModel.LateSubmitDays))
            {
                submitModel.SubmitStatus = (int)SubmitStatusEnum.Late;
            }
            else
            {
                submitModel.SubmitStatus = (int)SubmitStatusEnum.Overdue;
            }

            submitModel.Date = dateTime;

            return true;
        }

        public async Task<bool> ChangeEnrollAsync(HwSubmission submitModel, long enrollId)
        {
            if (submitModel == null) return false;

            var enrollment = await context.Enrollments.FindAsync(enrollId);
            if (enrollment == null) return false;

            var lessonModel = await context.Lessons.FindAsync(submitModel.Homework.LessonId);
            if (lessonModel == null) return false;

            if (lessonModel.ClassId == enrollment.ClassId && enrollment.StatusId == (int)EnrollEnum.Ongoing)
            {
                submitModel.EnrollId = enrollId;
                return true;
            }

            return false;
        }

        public Task<bool> ChangeFeedbackAsync(HwSubmission submitModel, string feedback)
        {
            if (submitModel == null) return Task.FromResult(false);

            submitModel.FeedBack = feedback;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeHomeworkAsync(HwSubmission submitModel, long homeworkId)
        {
            if (submitModel == null) return false;

            var homeworkModel = await context.Homework.FindAsync(homeworkId);
            if (homeworkModel == null) return false;

            var lessonModel = await context.Lessons.FindAsync(homeworkModel.LessonId);
            if (lessonModel == null) return false;

            if (submitModel.Enrollment.ClassId != lessonModel.ClassId) return false;

            submitModel.HomeworkId = homeworkId;

            return true;
        }

        public Task<bool> ChangeIsPassAsync(HwSubmission submitModel, bool isPass)
        {
            if (submitModel == null) return Task.FromResult(false);

            submitModel.IsPass = isPass;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeStatusAsync(HwSubmission submitModel, SubmitStatusEnum status)
        {
            if (submitModel == null) return Task.FromResult(false);

            submitModel.SubmitStatus = (int)status;

            return Task.FromResult(true);
        }
    }
}
