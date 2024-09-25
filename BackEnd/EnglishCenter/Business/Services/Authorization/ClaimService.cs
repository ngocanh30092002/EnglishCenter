using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EnglishCenter.Business.Services.Authorization
{
    public class ClaimService : IClaimService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClaimService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AddClaimToUserAsync(User user, ClaimDto model)
        {
            var userClaim = new Claim(model.ClaimName, model.ClaimValue);

            var result = await _userManager.AddClaimAsync(user, userClaim);

            return result.Succeeded;
        }

        public async Task<List<Claim>> GetClaimsUserAsync(User user)
        {
            var roleClaims = new List<Claim>();
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                var claimsOfRole = await GetRoleClaimsAsync(role);

                if (claimsOfRole == null) continue;

                roleClaims.AddRange(claimsOfRole);
            }

            var userClaims = await GetUserClaimsAsync(user);

            var claims = new List<Claim>();

            claims.AddRange(roleClaims);
            claims.AddRange(userClaims);

            return claims;
        }

        public async Task<List<Claim>> GetUserClaimsAsync(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            return userClaims.ToList();
        }

        public async Task<List<Claim>> GetRoleClaimsAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return null;
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            return roleClaims.ToList();
        }

        public async Task<bool> AddClaimInRoleAsync(string roleName, ClaimDto model)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null) return false;

            var result = await _roleManager.AddClaimAsync(role, new Claim(model.ClaimName, model.ClaimValue));

            return result.Succeeded;
        }

        public async Task<bool> DeleteClaimInRoleAsync(string roleName, ClaimDto model)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return false;

            var claim = new Claim(model.ClaimName, model.ClaimValue);
            var result = await _roleManager.RemoveClaimAsync(role, claim);

            return result.Succeeded;
        }

        public async Task<bool> DeleteClaimInUserAsync(User user, ClaimDto model)
        {
            var claim = new Claim(model.ClaimName, model.ClaimValue);
            var result = await _userManager.RemoveClaimAsync(user, claim);

            return result.Succeeded;
        }
    }
}
