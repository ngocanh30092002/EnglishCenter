using System.Security.Claims;
using EnglishCenter.Models;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IJsonWebTokenRepository
    {
        public Task<string> GenerateUserTokenAsync(User user, DateTime expireDate);
        public Task<DecodeJwtToken> DecodeToken(string token);   
    }
}
