using System.Security.Claims;
using EnglishCenter.Models;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IClaimRepository
    {
        public Task<List<Claim>> GetClaims(User user);
        public Task<List<Claim>> GetUserClaims(User user);
        public Task<List<Claim>> GetRoleClaims(User user);
        public Task<bool> AddClaimToUser(User user ,string claimName, string claimValue);
    }
}
