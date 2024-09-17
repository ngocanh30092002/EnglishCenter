using System.Security.Claims;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IClaimRepository
    {
        public Task<List<Claim>> GetRoleClaimsAsync(string roleName);
        public Task<List<Claim>> GetClaimsUserAsync(User user);
        public Task<List<Claim>> GetUserClaimsAsync(User user);
        public Task<bool> AddClaimToUserAsync(User user , ClaimDto model);
        public Task<bool> AddClaimInRoleAsync(string roleName, ClaimDto model);
        public Task<bool> DeleteClaimInRoleAsync(string roleName, ClaimDto model);
        public Task<bool> DeleteClaimInUserAsync(User user,  ClaimDto model);
    }
}
