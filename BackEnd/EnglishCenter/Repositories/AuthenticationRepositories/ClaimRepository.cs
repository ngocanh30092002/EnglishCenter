using System.Security.Claims;
using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Repositories.AuthenticationRepositories
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly UserManager<UserAccount> _useManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClaimRepository(UserManager<UserAccount> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _useManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AddClaimToUser(UserAccount user, string claimName, string claimValue)
        {
            var userInDatabase = await _useManager.FindByEmailAsync(user.Email!);

            if (userInDatabase == null) return false;

            var userClaim = new Claim(claimName, claimValue);

            var result = await _useManager.AddClaimAsync(user, userClaim);

            return result.Succeeded;
        }

        public async Task<List<Claim>> GetClaims(UserAccount user)
        {
            var roleClaims = await GetRoleClaims(user);
            var userClaims = await GetUserClaims(user);

            var claims = new List<Claim>();

            claims.AddRange(roleClaims);
            claims.AddRange(userClaims);

            return claims;
        }

        public async Task<List<Claim>> GetRoleClaims(UserAccount user)
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

        public async Task<List<Claim>> GetUserClaims(UserAccount user)
        {
            var userClaims = await _useManager.GetClaimsAsync(user);

            return userClaims.ToList(); 
        }
    }
}
