using EnglishCenter.Presentation.Models;

namespace EnglishCenter.Business.IServices
{
    public interface IRoleService
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
