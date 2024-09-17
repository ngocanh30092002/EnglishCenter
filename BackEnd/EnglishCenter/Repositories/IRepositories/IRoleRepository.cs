using EnglishCenter.Global;
using EnglishCenter.Models;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IRoleRepository
    {
        public Task<bool> CreateRoleAsync(string roleName);
        public Task<bool> IsExistRoleAsync(string roleName);
        public Task<bool> DeleteRoleAsync(string roleName);
        public Task<Response> GetRolesAsync();
        public Task<bool> AddUserRoleAsync(string userId, string roleName);
        public Task<Response> GetUserRolesAsync(string userId);
        public Task<bool> DeleteUserRolesAsync(string userId, string roleName);
    }
}
