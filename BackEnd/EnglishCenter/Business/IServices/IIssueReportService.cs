using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IIssueReportService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetById(long id);
        public Task<Response> GetByUserAsync(string userId);
        public Task<Response> GetAllByAdminAsync();
        public Task<Response> GetTypeAsync();
        public Task<Response> GetStatusAsync();
        public Task<Response> ChangeStatusAsync(long id, int status);
        public Task<Response> CreateAsync(IssueReportDto model);
        public Task<Response> UpdateAsync(long id, IssueReportDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
