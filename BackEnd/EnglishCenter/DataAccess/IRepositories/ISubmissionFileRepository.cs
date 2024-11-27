using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ISubmissionFileRepository : IGenericRepository<SubmissionFile>
    {
        public Task<bool> ChangeFilePathAsync(SubmissionFile fileModel, string newPath);
        public Task<bool> ChangeLinkUrlAsync(SubmissionFile fileModel, string newLinkUrl);
        public Task<bool> ChangeUploadByAsync(SubmissionFile fileModel, string uploadBy);
        public Task<bool> ChangeSubmissionTaskAsync(SubmissionFile fileModel, long submitTaskId);
    }
}
