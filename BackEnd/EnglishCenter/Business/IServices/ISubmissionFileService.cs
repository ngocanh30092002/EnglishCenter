using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface ISubmissionFileService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetBySubmissionAsync(long submissionId);
        public Task<Response> GetByEnrollAndIdAsync(long enrollId, long submissionTaskId);
        public Task<Response> ChangeFilePathAsync(long id, IFormFile file);
        public Task<Response> ChangeLinkUrlAsync(long id, string newLinkUrl);
        public Task<Response> ChangeUploadByAsync(long id, string uploadBy);
        public Task<Response> ChangeSubmissionTaskAsync(long id, long submitTaskId);
        public Task<Response> HandleUploadMoreFilesAsync(string userId, SubmissionFileDto model);
        public Task<Response> CreateAsync(string userId, SubmissionFileDto model);
        public Task<Response> UpdateAsync(long id, SubmissionFileDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
