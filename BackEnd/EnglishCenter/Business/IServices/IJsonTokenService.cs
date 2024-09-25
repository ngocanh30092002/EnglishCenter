using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;

namespace EnglishCenter.Business.IServices
{
    public interface IJsonTokenService
    {
        public Task<string> GenerateUserTokenAsync(User user, DateTime expireDate, Provider provider = 0);
        public Task<DecodeJwtToken> DecodeToken(string token);
        public string GenerateRefreshToken();
        public Task<Response> RenewTokenAsync(string accessToken, string refreshToken);
        public Task<string> GetRefreshTokenFromUser(User user, Provider provider = Provider.System);
        public bool VerifyAccessToken(string accessToken);
    }
}
