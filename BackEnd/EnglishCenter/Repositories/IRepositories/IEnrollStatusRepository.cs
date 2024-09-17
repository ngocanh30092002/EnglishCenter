using EnglishCenter.Models;
using EnglishCenter.Models.DTO;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IEnrollStatusRepository
    {
        public Response CreateEnrollStatus(EnrollStatusDto model);
        public Task<Response> UpdateEnrollStatusAsync(int enrollStatusId,  EnrollStatusDto model);
        public Task<Response> DeleteEnrollStatusAsync(int enrollStatusId);
        public Task<Response> GetAllEnrollStatusAsync();
        public Task<Response> GetEnrollStatusAsync(int enrollStatusId);
    }
}
