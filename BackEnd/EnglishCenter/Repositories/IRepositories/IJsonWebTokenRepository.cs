using System.Security.Claims;
using EnglishCenter.Global.Enum;
using EnglishCenter.Models;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IJsonWebTokenRepository
    {
        public Task<string> GenerateUserTokenAsync(User user, DateTime expireDate, Provider provider = 0);
        public Task<DecodeJwtToken> DecodeToken(string token);
        public string GenerateRefreshToken();
        public Task<Response> RenewTokenAsync(string accessToken, string refreshToken);
        public Task<string> GetRefreshTokenFromUser(User user, Provider provider = Provider.System);
        public bool VerifyAccessToken(string accessToken);
    }
}
