using EnglishCenter.Models;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IAdminRepository
    {
        public Task<Response> AddRoleToUserAsync(string userId, string roleName);
        public Task<Response> AddClaimToUserAsync(string userId, string claimName, string claimValue);
    }
}
