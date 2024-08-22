using System.Security.Claims;
using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Repositories.AuthenticationRepositories
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly UserManager<User> _useManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClaimRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _useManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AddClaimToUser(User user, string claimName, string claimValue)
        {
            if (string.IsNullOrEmpty(claimName) || string.IsNullOrEmpty(claimValue)) return false;
            
            var userInDatabase = await _useManager.FindByEmailAsync(user.Email!);
            if (userInDatabase == null) return false;

            var userClaim = new Claim(claimName, claimValue);

            var result = await _useManager.AddClaimAsync(user, userClaim);

            return result.Succeeded;
        }

        public async Task<List<Claim>> GetClaims(User user)
        {
            var roleClaims = await GetRoleClaims(user);
            var userClaims = await GetUserClaims(user);

            var claims = new List<Claim>();

            claims.AddRange(roleClaims);
            claims.AddRange(userClaims);

            return claims;
        }

        public async Task<List<Claim>> GetRoleClaims(User user)
        {
            var claims = new List<Claim>();
            var userRoles = await _useManager.GetRolesAsync(user);

            foreach(var role in userRoles) 
            {
                var roleClaims = await _roleManager.GetClaimsAsync(new IdentityRole(role));

                if (roleClaims == null) continue;

                claims.AddRange(roleClaims);
            }

            return claims;
        }

        public async Task<List<Claim>> GetUserClaims(User user)
        {
            var userClaims = await _useManager.GetClaimsAsync(user);

            return userClaims.ToList(); 
        }
    }
}
