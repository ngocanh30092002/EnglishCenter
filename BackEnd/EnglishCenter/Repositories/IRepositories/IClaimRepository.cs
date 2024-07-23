using System.Security.Claims;
using EnglishCenter.Models;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IClaimRepository
    {
        public Task<List<Claim>> GetClaims(UserAccount user);
        public Task<List<Claim>> GetUserClaims(UserAccount user);
        public Task<List<Claim>> GetRoleClaims(UserAccount user);
        public Task<bool> AddClaimToUser(UserAccount user ,string claimName, string claimValue);
    }
}
