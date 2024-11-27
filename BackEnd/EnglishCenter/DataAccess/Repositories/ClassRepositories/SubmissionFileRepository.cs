using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.DataAccess.Repositories.ClassRepositories
{
    public class SubmissionFileRepository : GenericRepository<SubmissionFile>, ISubmissionFileRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SubmissionFileRepository(EnglishCenterContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task<bool> ChangeFilePathAsync(SubmissionFile fileModel, string newPath)
        {
            if (fileModel == null) return Task.FromResult(false);

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, newPath);
            if (!File.Exists(filePath)) return Task.FromResult(false);

            fileModel.FilePath = newPath;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeLinkUrlAsync(SubmissionFile fileModel, string newLinkUrl)
        {
            if (fileModel == null) return Task.FromResult(false);

            fileModel.LinkUrl = newLinkUrl;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeSubmissionTaskAsync(SubmissionFile fileModel, long submitTaskId)
        {
            if (fileModel == null) return false;

            var submitTask = await context.SubmissionTask.FindAsync(submitTaskId);
            if (submitTask == null) return false;

            fileModel.Status = (submitTask.StartTime <= fileModel.UploadAt && fileModel.UploadAt <= submitTask.EndTime) ? (int)SubmissionFileEnum.OnTime : (int)SubmissionFileEnum.Late;
            fileModel.SubmissionTaskId = submitTaskId;

            return true;
        }

        public Task<bool> ChangeUploadByAsync(SubmissionFile fileModel, string uploadBy)
        {
            if (fileModel == null) return Task.FromResult(false);

            fileModel.UploadBy = uploadBy;

            return Task.FromResult(true);
        }
    }
}
