using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IEnrollStatusRepository : IGenericRepository<EnrollStatus>
    {
        public Task<bool> UpdateAsync(int enrollStatusId, EnrollStatus model);
    }
}
