using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Repositories.AdminRepositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IRoleRepository _roleRepo;
        private readonly IClaimRepository _claimRepo;
        private readonly UserManager<User> _userManager;

        public AdminRepository(IRoleRepository roleRepo, IClaimRepository claimRepo, UserManager<User> userManager ) 
        {
            _roleRepo = roleRepo;
            _claimRepo = claimRepo;
            _userManager = userManager;
        }
        public async Task<Response> AddClaimToUserAsync(string userId, string claimName, string claimValue)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response()
                {
                    Message = "Can't find any users",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var isSuccess = await _claimRepo.AddClaimToUserAsync(user, new ClaimDto(claimName, claimValue));

            if (!isSuccess)
            {
                return new Response()
                {
                    Message = "Can't add claim to user",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new Response()
            {
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<Response> AddRoleToUserAsync(string userId, string roleName)
        {
            var isSuccess = await _roleRepo.AddUserRoleAsync(userId, roleName);

            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't add role in this user"
                };
            }

            return new Response()
            {
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true
            };
        }
    }
}
