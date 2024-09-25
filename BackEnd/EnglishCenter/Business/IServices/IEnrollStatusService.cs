using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IEnrollStatusService 
    {
        public Task<Response> CreateAsync(EnrollStatusDto model);
        public Task<Response> UpdateAsync(int enrollStatusId, EnrollStatusDto model);
        public Task<Response> DeleteAsync(int enrollStatusId);
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(int enrollStatusId);
    }
}
