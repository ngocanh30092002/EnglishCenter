using EnglishCenter.Presentation.Models;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAdminRepository
    {
        public Task<Response> AddRoleToUserAsync(string userId, string roleName);
        public Task<Response> AddClaimToUserAsync(string userId, string claimName, string claimValue);
    }
}
