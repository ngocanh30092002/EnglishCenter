using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
namespace EnglishCenter.Business.IServices
{
    public interface IIssueResponseService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetById(long id);
        public Task<Response> CreateAsync(IssueResponseDto model);
        public Task<Response> UpdateAsync(long id, IssueResponseDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
