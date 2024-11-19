using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Authorization
{
    public class RoleService : IRoleService
    {
        private RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> AddUserRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return false;
            }

            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var isExisted = await IsExistRoleAsync(roleName);

            if (isExisted)
            {
                return false;
            }

            await _roleManager.CreateAsync(new IdentityRole(roleName));
            return true;
        }

        public async Task<bool> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return false;
            }

            await _roleManager.DeleteAsync(role);
            return true;
        }

        public async Task<bool> DeleteUserRolesAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return false;
            }

            await _userManager.RemoveFromRoleAsync(user, roleName);
            return true;
        }

        public async Task<Response> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return new Response()
            {
                Message = roles.Select(r => r.Name).ToList(),
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response()
                {
                    Message = "Can't find any users",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                }; ;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            return new Response()
            {
                Message = userRoles,
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<bool> IsExistRoleAsync(string roleName)
        {
            var isExistRole = await _roleManager.Roles.FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper());

            return isExistRole != null;
        }
    }
}
